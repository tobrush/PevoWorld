using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Split Ratio", menuName = "Camera Control/Split Ratio")]
public class SplitRatioControl : CameraControlBase
{
    [Header("Controls")]
    [Range(0, 100)] public int CameraPercent = 50;

    public override void Activate(Transform playerTransform)
    {
        SplitScreenManager.Instance.SetSplitCameraRatio(playerTransform, ((float)CameraPercent / 50) - 1, ActualTransitionTime);
    }
}