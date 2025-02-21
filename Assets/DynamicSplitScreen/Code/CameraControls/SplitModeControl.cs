using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Split Mode", menuName = "Camera Control/Split Mode")]
public class SplitModeControl : CameraControlBase
{
    [Header("Controls")]
    /// <summary>
    /// If false, game will transition into split screen if possible, otherwise single player camera will be rendered.
    /// </summary>
    public bool SoloMode = false;

    public override void Activate(Transform playerTransform)
    {
        if (SplitScreenManager.Instance.SoloMode == SoloMode) return;

        if (SplitScreenManager.Instance.SoloMode)
        {
            SplitScreenManager.Instance.SetSplitMode();
        }
        else
        {
            SplitScreenManager.Instance.SetSoloMode(playerTransform);
        }
    }
}
