// Defines for rendering pipeline - if LWRP, URP and HDRP are false, standard rendering will be used
//#define URP
//#define HDRP
//#define LWRP

#if LWRP || URP || HDRP
#define MODERN_RENDERING
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Rendering;
#if LWRP
using RP = UnityEngine.Rendering.LWRP;
#elif URP
using RP = UnityEngine.Rendering.Universal;
#elif HDRP
using RP = UnityEngine.Rendering.HighDefinition;
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


namespace DynamicSplitScreen
{
    public class SplitScreenManager : SingletonBehaviour<SplitScreenManager>
    {
        // constants
        public const int PLAYER_HUD_LAYER_ORDER = 100;

        [Header("Rendering")]
        public Material SplitScreenMaterial;
        public LayerMask NormalMask;
        public LayerMask TopLayerMask;

#if LWRP || URP
        public RP.ScriptableRendererData rendererData;
#endif

#if MODERN_RENDERING
        private SplitScreenRendererPass splitScreenPass;
#endif

        [Header("Prefabs")]
        public GameObject PlayerCameraPrefab;

        [Header("Default settings")]
        [SerializeField] private float cameraDistance = 10;
        public float MinimumSplitScreenDistanceMultiplier = .75f; // defines at what percentage of minimum split screen distance is split screen created (for smooth transition)
        public bool LockSplitScreen = false;
        public bool LockSplitAngle = false;
        public bool OverrideSplitAngle = false;
        [Range(0, 360)] public float SplitAngle = 0; // 0 is horizontal, 90 player 1 on left side and so on

        [Header("Extra 3D Controls")]
        public bool Is3D = false; // don't change during runtime (why would you even...)
        [Range(45, 90)] public float XAngle = 90;

        // references
        private SplitScreenController splitScreen;
        private Camera splitScreenCamera;
        private SplitWorldCamera splitCamera;
        private PlayerCamera player1Camera;
        private PlayerCamera player2Camera;
        private Transform player1;
        private Transform player2;

        // split screen camera controls
        private bool splitScreenLocked = false;
        private TimedTransition splitRatioTransition;
        private TimedTransition cameraDistanceTransition;
        private TimedTransition orthoSizeTransition;
        private TimedTransition xAngleTransition;
        private TimedTransition splitAngleTransition;

        // screen resolution settings
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }
        private float aspectRatio;

        public static Camera MainCamera
        {
            get
            {
                if (Instance)
                {
                    return Instance.splitScreenCamera;
                }
                else
                {
                    return GameObject.FindObjectOfType<Camera>();
                }
            }
        }

        public static float OrthoSize
        {
            get
            {
                return Instance ? Instance.splitScreenCamera.orthographicSize : 0;
            }
        }

        public static float CameraDistance
        {
            get
            {
                return Instance ? Instance.cameraDistance : 0;
            }
        }

        public Action<int, int> OnResolutionChanged;

        public void RegisterSplitLine()
        {
            SplitLine splitLine = SplitLine.Instance;
            splitLine.RescaleSplitLine(ScreenWidth, ScreenHeight);
            splitScreen.SplitLine = splitLine;
            TryEnableSplitScreen();
        }

        public void RegisterPlayer(Transform player)
        {
            if (player1 == null)
            {
                player1 = player.transform;
                GameObject cam1 = Instantiate(PlayerCameraPrefab);
                cam1.name = "Player 1 camera";
                player1Camera = cam1.GetComponent<PlayerCamera>();
                splitScreen.RegisterCamera(1, player1Camera);
            }
            else if (player2 == null)
            {
                player2 = player.transform;
                GameObject cam2 = Instantiate(PlayerCameraPrefab);
                cam2.name = "Player 2 camera";
                player2Camera = cam2.GetComponent<PlayerCamera>();
                splitScreen.RegisterCamera(2, player2Camera);
            }

            TryEnableSplitScreen();
        }

        public void UnregisterPlayer(Transform player)
        {
            if (player1 == player)
            {
                player1 = null;
            }
            else if (player2 == player)
            {
                player2 = null;
            }

            splitScreen.EnableSplitScreen(false);
        }

        public bool IsTransformRegisteredPlayer(Transform player)
        {
            return player == player1 || player == player2;
        }

        private void TryEnableSplitScreen()
        {
            if (player1 && player2 && SplitLine.Instance)
            {
                splitScreen.EnableSplitScreen(true);
                FadeManager.BeginFadeAllFromColor(1);
            }
        }

        private void SetScreenProperties()
        {
            // call after changing resolution
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;
            aspectRatio = (float)ScreenWidth / (float)ScreenHeight;

            OnResolutionChanged?.Invoke(ScreenWidth, ScreenHeight);
        }

        #region Camera control
        public void SetSplitCameraRatio(Transform player, float ratio, float transitionTime = 0)
        {
            if (player == player1)
            {
                splitRatioTransition.StartTransition(ratio, transitionTime);
            }
            else if (player == player2)
            {
                splitRatioTransition.StartTransition(-ratio, transitionTime);
            }
        }

        public void SetPlayerCameraOrthoSize(Transform player, float size, float transitionTime = 0)
        {
            if (player == player1)
            {
                player1Camera.SetCameraOrthoSize(size, transitionTime);
            }
            else if (player == player2)
            {
                player2Camera.SetCameraOrthoSize(size, transitionTime);
            }
        }

        public void SetMainCameraOrthoSize(float size, float transitionTime = 0)
        {
            orthoSizeTransition.StartTransition(size, transitionTime);
        }

        public void SetPlayerCameraDistance(Transform player, float distance, float transitionTime = 0)
        {
            if (player == player1)
            {
                player1Camera.SetCameraDistance(distance, transitionTime);
            }
            else if (player == player2)
            {
                player2Camera.SetCameraDistance(distance, transitionTime);
            }
        }

        public void SetMainCameraDistance(float distance, float transitionTime = 0)
        {
            cameraDistanceTransition.StartTransition(distance, transitionTime);
        }

        public void ResetSplitCameraSize()
        {
            splitRatioTransition = new TimedTransition(0);
        }

        public void SetCameraXAngle(float xAngle, float transitionTime = 0)
        {
            if (Is3D) xAngleTransition.StartTransition(xAngle, transitionTime);
        }

        public void SetOverrideSplitAngle(Transform triggeredByPlayer, float splitAngle, float transitionTime = 0)
        {
            splitAngleTransition = new TimedTransition(SplitAngle);
            splitAngle += triggeredByPlayer == player2 ? 180 : 0;
            splitAngle %= 360;
            float shortestTransition = splitAngle - SplitAngle;
            shortestTransition = shortestTransition > 180 ? shortestTransition - 360 : shortestTransition;
            shortestTransition = shortestTransition < -180 ? shortestTransition + 360 : shortestTransition;
            splitAngleTransition.StartTransition(SplitAngle + shortestTransition, transitionTime);
            splitAngleTransition.OnCurrentValueChanged = SetSplitAngle;
        }

        public bool SoloMode { get; private set; }
        private Transform soloPlayer = null;

        public void SetSoloMode(Transform player)
        {
            splitScreen.EnableSplitScreen(false);
            soloPlayer = player;
            SoloMode = true;
        }

        public void SetSplitMode()
        {
            splitScreen.EnableSplitScreen(true);
            soloPlayer = null;
            SoloMode = false;
        }

        private void SetCameraOrthoSize(float size)
        {
            splitScreenCamera.orthographicSize = size;
        }

        private void SetXAngle(float x)
        {
            XAngle = x;
        }

        private void SetSplitAngle(float angle)
        {
            SplitAngle = angle;
        }
        #endregion

        #region Rendering

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            SetScreenProperties();

            splitScreenCamera = GetComponent<Camera>();
            splitScreenCamera.transform.rotation = Is3D ? Quaternion.Euler(90, 0, 0) : Quaternion.Euler(0, 0, 0);
            splitScreenCamera.orthographic = !Is3D;

#if LWRP || URP
            if (rendererData == null)
            {
                Debug.LogError("No renderer data provided for LWRP.");
                enabled = false;
                return;
            }

            splitScreenPass = (SplitScreenRendererPass)rendererData.rendererFeatures.Find(p => p.GetType() == typeof(SplitScreenRendererPass));

            if (splitScreenPass == null)
            {
                Debug.LogError("No SplitScreenRendererPass found in renderer data.");
                enabled = false;
                return;
            }
#endif

            SplitScreenMaterial = Instantiate(SplitScreenMaterial);

            splitScreen = SplitScreenController.GetController(Is3D);
            splitScreen.Initialize(this, SplitScreenMaterial);
            splitScreen.OnSplitScreenStateChanged += OnSplitStateChanged;

            splitCamera = GetComponentInChildren<SplitWorldCamera>();
#if !MODERN_RENDERING
            splitCamera.SplitRT = splitScreen.splitRT;
#else
            splitCamera.gameObject.SetActive(false);
#endif

            splitRatioTransition = new TimedTransition(0);
            cameraDistanceTransition = new TimedTransition(CameraDistance);
            orthoSizeTransition = new TimedTransition(OrthoSize);
            orthoSizeTransition.OnCurrentValueChanged += SetCameraOrthoSize;
            xAngleTransition = new TimedTransition(XAngle);
            xAngleTransition.OnCurrentValueChanged += SetXAngle;
            splitAngleTransition = new TimedTransition(SplitAngle);
        }

        private void OnSplitStateChanged(SplitScreenState state)
        {
            if (splitScreenCamera)
            {
                bool isSplit = state == SplitScreenState.Split;
                splitScreenCamera.cullingMask = isSplit ? TopLayerMask : NormalMask;
                splitScreenCamera.clearFlags = isSplit ? CameraClearFlags.Nothing : CameraClearFlags.Skybox;

#if !MODERN_RENDERING
                splitCamera.enabled = isSplit;
#endif
            }
        }

        private void Update()
        {
            if (Screen.width != ScreenWidth || Screen.height != ScreenHeight)
            {
                SetScreenProperties();
            }

            if (!player1 && !player2)
            {
                return;
            }

            bool singlePlayer = !player1 || !player2;

            // reset splitAngleLocked
            if (!LockSplitScreen && splitScreenLocked)
            {
                splitScreenLocked = false;
            }

            Vector3 position1 = player1 ? player1.position : player2.position;
            Vector3 position2 = player2 ? player2.position : player1.position;

            if (SoloMode)
            {
                position1 = soloPlayer.position;
                position2 = position1;
            }

            splitScreen.UpdateCameraDistance(cameraDistanceTransition.CurrentValue);
            splitScreen.Update(cameraDistanceTransition.CurrentValue,
                position1,
                position2,
                splitRatioTransition.CurrentValue,
                singlePlayer || SoloMode,
                ref splitScreenLocked, ref SplitAngle);

            Render();
        }

        private void Render()
        {
            bool isSplit = splitScreen.SplitScreenState == SplitScreenState.Split;

#if MODERN_RENDERING
            splitScreenPass.SetMode(!isSplit, splitScreen.splitRT);
#endif

            if (!isSplit)
            {
                return;
            }

            splitScreen.Render();
        }

        public static int GetAntialiasingLevel()
        {           
#if LWRP
            var lwrp = GraphicsSettings.renderPipelineAsset as RP.LightweightRenderPipelineAsset;
            return lwrp.msaaSampleCount;
#elif URP
            var urp = GraphicsSettings.renderPipelineAsset as RP.UniversalRenderPipelineAsset;
            return urp.msaaSampleCount;
#else
            return QualitySettings.antiAliasing;
#endif
        }
        #endregion

        #region Editor
#if UNITY_EDITOR
        [MenuItem("DynamicSplitScreen/Setup 2D")]
        private static void Setup2DSplitScreen()
        {
            SetupSplitScreen(false);
        }

        [MenuItem("DynamicSplitScreen/Setup 3D")]
        private static void Setup3DSplitScreen()
        {
            SetupSplitScreen(true);
        }

        private static void SetupSplitScreen(bool is3D)
        {
            string[] assetGUIDs = AssetDatabase.FindAssets(is3D ? "SplitScreenCamera3D" : "SplitScreenCamera2D");

            bool instantiated = false;
            foreach (string guid in assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                PrefabUtility.InstantiatePrefab(prefab);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                instantiated = true;
                break;
            }

            if (!instantiated)
            {
                Debug.LogError("Couldn't find split screen camera prefab");
                return;
            }

            instantiated = false;
            assetGUIDs = AssetDatabase.FindAssets("SplitScreenUI");
            foreach (string guid in assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                PrefabUtility.InstantiatePrefab(prefab);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                instantiated = true;
                break;
            }

            if (!instantiated)
            {
                Debug.LogError("Couldn't find split screen UI prefab");
                return;
            }
        }
#endif
        #endregion
    }

    public enum SplitScreenState
    {
        Disabled,

        /// <summary>
        /// in split screen mode, before actual state has been specified
        /// </summary>
        Enabled,

        Single,
        Split
    }
}