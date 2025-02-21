using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSplitScreen
{
    /// <summary>
    /// Class takes care of rendering split screen.
    /// </summary>
    public abstract class SplitScreenController
    {
        protected const int PLAYER_1_INDEX = 0;
        protected const int PLAYER_2_INDEX = 1;

        // references
        protected SplitScreenManager splitManager;
        protected Camera splitScreenCamera;
        protected PlayerCamera[] playerCamera = new PlayerCamera[2];
        protected Camera[] playerCameraComponent = new Camera[2];
        protected Transform[] playerCameraTransform = new Transform[2];

        // public - set by SplitScreenManager
        public SplitLine SplitLine;

        // rendering
        private Material splitScreenMat;
        public RenderTexture splitRT;

        // split calculations
        protected bool enableSplitScreen = true;
        protected Vector2 centerOffset;
        protected float minimumSplitScreenDistance;
        protected Vector3 worldUp;
        protected Vector3 cameraDistanceVector;
        protected Vector3 lastPlayer1CameraPositionOffset;
        protected Vector3 lastPlayer2CameraPositionOffset;
        protected float lastCam1Distance;
        protected float lastCam2Distance;

        // transitions
        protected float transitionSplitRatio;
        protected float transitionAspectRatio;
        protected float smoothTransitionRatio;

        // screen properties
        protected int screenX;
        protected int screenY;
        protected float aspectRatio;

        public Action<SplitScreenState> OnSplitScreenStateChanged;
        private SplitScreenState splitScreenState = SplitScreenState.Disabled;
        public SplitScreenState SplitScreenState
        {
            get
            {
                return splitScreenState;
            }
            protected set
            {
                if (value != splitScreenState)
                {
                    if (SplitLine) SplitLine.Enabled = value == SplitScreenState.Split;
                    if (playerCamera[PLAYER_1_INDEX]) playerCamera[PLAYER_1_INDEX].enabled = value == SplitScreenState.Split;
                    if (playerCamera[PLAYER_2_INDEX]) playerCamera[PLAYER_2_INDEX].enabled = value == SplitScreenState.Split;

                    splitScreenState = value;
                    OnSplitScreenStateChanged?.Invoke(value);
                }
            }
        }

        public virtual void Initialize(SplitScreenManager manager, Material split)
        {
            splitManager = manager;
            splitScreenCamera = SplitScreenManager.MainCamera;
            splitScreenMat = split;

            screenX = SplitScreenManager.ScreenWidth;
            screenY = SplitScreenManager.ScreenHeight;
            aspectRatio = (float)screenX / screenY;

            splitManager.OnResolutionChanged += (x, y) =>
            {
                screenX = x;
                screenY = y;
                aspectRatio = (float)x / y;
            };

            splitManager.OnResolutionChanged += CreateRenderTextures;
            CreateRenderTextures(SplitScreenManager.ScreenWidth, SplitScreenManager.ScreenHeight);
        }

        ~SplitScreenController()
        {
            splitManager.OnResolutionChanged -= CreateRenderTextures;
        }

        protected virtual void CreateRenderTextures(int width, int height)
        {

        }

        /// <summary>
        /// Sets <see cref="SplitScreenState"/> to <see cref="SplitScreenState.Enabled"/> or <see cref="SplitScreenState.Disabled"/> based on <paramref name="enable"/>
        /// When enabled, split screen will render single player screen or split screen, depending on player distance if both players are available.
        /// </summary>
        /// <param name="enable"></param>
        public void EnableSplitScreen(bool enable)
        {
            enableSplitScreen = enable;
            SplitScreenState = enable ? SplitScreenState.Enabled : SplitScreenState.Disabled;
        }

        public void UpdateCameraDistance(float distance)
        {
            cameraDistanceVector = worldUp * distance;
        }

        public abstract void Update(float CameraDistance, Vector3 player1Position, Vector3 player2Position, float splitRatio, bool singlePlayer, ref bool splitAngleLocked, ref float splitAngle);

        /// <summary>
        /// Renders player cameras to RenderTexture
        /// </summary>
        public virtual void Render()
        {
            // render both player screens to splitRT
            RenderTexture.active = splitRT;
            GL.Clear(true, true, new Color(0, 0, 0, 0));

            playerCameraComponent[PLAYER_1_INDEX].Render();
            splitScreenMat.SetTexture("_MainTex", playerCameraComponent[PLAYER_1_INDEX].targetTexture);

            playerCameraComponent[PLAYER_2_INDEX].Render();
            splitScreenMat.SetTexture("_SecondaryTex", playerCameraComponent[PLAYER_2_INDEX].targetTexture);

            SplitLine.SplitBackgroundCamera.Render();
            splitScreenMat.SetTexture("_BackgroundTex", SplitLine.BackgroundRender);

            Graphics.Blit(null, splitScreenMat);
            RenderTexture.active = null;
        }

        /// <summary>
        /// Register camera for player with specified player index.
        /// </summary>
        /// <param name="playerIndex">player index should be in range 1-2</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public virtual bool RegisterCamera(int playerIndex, PlayerCamera camera)
        {
            playerIndex--;

            if (playerIndex >= 0 && playerIndex < 2)
            {
                playerCamera[playerIndex] = camera;
                playerCameraComponent[playerIndex] = camera.Camera;
                playerCameraTransform[playerIndex] = camera.transform;

                playerCameraTransform[playerIndex].rotation = splitScreenCamera.transform.rotation;

                return true;
            }
            else
            {
                Debug.LogError("Wrong index passed to SplitScreenController.RegisterCamera()");

                return false;
            }
        }

        public static SplitScreenController GetController(bool is3D)
        {
            return is3D ? (SplitScreenController)new SplitScreenController3D() : (SplitScreenController)new SplitScreenController2D();
        }
    }
}
