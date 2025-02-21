using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicSplitScreen
{
    public class SplitLine : SingletonBehaviour<SplitLine>
    {
        [Header("References")]
        public Camera SplitBackgroundCamera;
        public Image SplitLineImage;
        public Image SplitLineBackground;

        [Header("Custom Split Line")]
        public Color LineColor;
        public Sprite SplitLineSprite;
        public Sprite BackgroundSprite;

        public Vector3 Position
        {
            get { return SplitLineImage.rectTransform.localPosition; }
            set
            {
                SplitLineImage.rectTransform.localPosition = value;
                SplitLineBackground.rectTransform.localPosition = value;
            }
        }

        public Quaternion Rotation
        {
            get { return SplitLineImage.transform.localRotation; }
            set
            {
                SplitLineImage.transform.localRotation = value;
                SplitLineBackground.transform.localRotation = value;
            }
        }

        private bool enable = false;
        public bool Enabled
        {
            get { return enable; }
            set
            {
                SplitLineImage.enabled = value;
                SplitLineBackground.enabled = value;
            }
        }

        private RenderTexture backgroundRender;
        public RenderTexture BackgroundRender
        {
            get { return backgroundRender; }
        }

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            SplitBackgroundCamera.enabled = false;
            Enabled = false;

            SplitLineImage.color = LineColor;
            SplitLineImage.sprite = SplitLineSprite;
            SplitLineBackground.sprite = BackgroundSprite;

            var canvas = SplitLineImage.GetComponentInParent<Canvas>();
            canvas.worldCamera = SplitScreenManager.MainCamera;
            canvas.planeDistance = 0.1f;

            if (!SplitScreenManager.Instance.Is3D && SortingLayer.IsValid(SortingLayer.NameToID("Foreground")))
            {
                canvas.sortingLayerName = "Foreground";
            }
            else if (!SplitScreenManager.Instance.Is3D)
            {
                Debug.LogWarning("'Foreground' layer wasn't found. Please read the README and create sorting layers called Foreground and Background.");
            }

            canvas.sortingOrder = SplitScreenManager.PLAYER_HUD_LAYER_ORDER;

            SplitScreenManager.Instance.RegisterSplitLine();
            SplitScreenManager.Instance.OnResolutionChanged += RescaleSplitLine;
        }

        private void OnDestroy()
        {
            SplitScreenManager.Instance.OnResolutionChanged -= RescaleSplitLine;
        }

        public void RescaleSplitLine(int width, int height)
        {
            float safeMultiplier = 2.5f;
            float minSplitHeight = Mathf.Sqrt(width * width * safeMultiplier + height * height * safeMultiplier);
            SplitLineImage.rectTransform.sizeDelta = new Vector2(minSplitHeight, minSplitHeight);
            SplitLineBackground.rectTransform.sizeDelta = new Vector2(minSplitHeight, minSplitHeight);

            // remake RenderTexture for splitBackgroundCamera
            backgroundRender = new RenderTexture(width, height, 0);
            SplitBackgroundCamera.targetTexture = backgroundRender;
        }
    }
}