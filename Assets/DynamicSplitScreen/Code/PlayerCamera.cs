using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSplitScreen
{
    public class PlayerCamera : MonoBehaviour
    {
        // references
        public GameObject PlayerHUD;
        private new Camera camera;
        public Camera Camera
        {
            get
            {
                if (!camera)
                {
                    camera = GetComponent<Camera>();
                }
                return camera;
            }
        }

        /// <summary>
        /// Determines how much are we into transition to split screen (0 = split screen off, 1 = fully transitioned into split screen)
        /// </summary>
        public float TransitionRatio { get; set; }
        public float CameraDistance { get; private set; }

        // private variables
        private RenderTexture renderTexture;
        private TimedTransition orthoSizeTransition;
        private TimedTransition cameraDistanceTransition;
        private bool freezeOrthoTransition = false;
        private float orthoSize;

        private void Awake()
        {
            this.Camera.enabled = false;

            DontDestroyOnLoad(gameObject);

            InitializeRenderTexture(SplitScreenManager.ScreenWidth, SplitScreenManager.ScreenHeight);
            TransitionRatio = 0;
            orthoSizeTransition = new TimedTransition(SplitScreenManager.OrthoSize);
            cameraDistanceTransition = new TimedTransition(SplitScreenManager.CameraDistance);

            PlayerHUD = Instantiate(PlayerHUD);
            DontDestroyOnLoad(PlayerHUD);
            StartCoroutine(InitializeHUD());

            SplitScreenManager.Instance.OnResolutionChanged += InitializeRenderTexture;
        }

        private void OnDestroy()
        {
            SplitScreenManager.Instance.OnResolutionChanged -= InitializeRenderTexture;
        }

        private void InitializeRenderTexture(int width, int height)
        {
            renderTexture = new RenderTexture(width, height, SplitScreenManager.Instance.Is3D ? 24 : 0);

            int msaa = SplitScreenManager.GetAntialiasingLevel();
            if (msaa > 1) renderTexture.antiAliasing = msaa;

            Camera.targetTexture = renderTexture;
        }

        private IEnumerator InitializeHUD()
        {
            yield return null;
            PlayerHUD.GetComponent<Canvas>().worldCamera = Camera;
            PlayerHUD.GetComponent<Canvas>().planeDistance = 0.1f;

            if (!SplitScreenManager.Instance.Is3D && SortingLayer.IsValid(SortingLayer.NameToID("Foreground")))
            {
                PlayerHUD.GetComponent<Canvas>().sortingLayerName = "Foreground";
            }
            else if (!SplitScreenManager.Instance.Is3D)
            {
                Debug.LogWarning("'Foreground' layer wasn't found. Please read the README and create sorting layers called Foreground and Background.");
            }

            PlayerHUD.GetComponent<Canvas>().sortingOrder = SplitScreenManager.PLAYER_HUD_LAYER_ORDER;
            yield return null;
            FadeManager.BeginFadeFromColor(Camera, 1);
        }

        private void LateUpdate()
        {
          
            // update ortho size
            if (!freezeOrthoTransition && camera.orthographic)
            {
                orthoSize = Mathf.Lerp(SplitScreenManager.OrthoSize, orthoSizeTransition.CurrentValue, TransitionRatio);
                camera.orthographicSize = orthoSize;
            }
        }

        /// <summary>
        /// Applies position to this camera so that it smoothly changes distance based on <see cref="TransitionRatio"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="worldUp"></param>
        /// <param name="mainCameraDistance"></param>
        /// <param name="lockSplitScreen"></param>
        public void UpdatePosition(Vector3 position, Vector3 worldUp, float mainCameraDistance, bool lockSplitScreen)
        {
            if (!lockSplitScreen) CameraDistance = Mathf.Lerp(mainCameraDistance, cameraDistanceTransition.CurrentValue, TransitionRatio);
            freezeOrthoTransition = lockSplitScreen;

            transform.position = position;
            transform.position += worldUp * CameraDistance;
        }

        public void SetCameraOrthoSize(float size, float transitionTime)
        {
            orthoSizeTransition.StartTransition(size, transitionTime);
        }

        public void SetCameraDistance(float distance, float transitionTime)
        {
            cameraDistanceTransition.StartTransition(distance, transitionTime);
        }
    }
}