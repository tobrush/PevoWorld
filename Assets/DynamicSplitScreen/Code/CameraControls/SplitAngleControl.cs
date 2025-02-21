using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Split Angle", menuName = "Camera Control/Split Angle")]
public class SplitAngleControl : CameraControlBase
{
    [Header("Controls")]
    public bool OverrideSplitAngle = false;
    [Range(0, 360)] public float NewSplitAngle = 0;

    public override void Activate(Transform playerTransform)
    {
        SplitScreenManager.Instance.LockSplitAngle = OverrideSplitAngle;
        SplitScreenManager.Instance.OverrideSplitAngle = OverrideSplitAngle;

        if (OverrideSplitAngle)
        {
            SplitScreenManager.Instance.SetOverrideSplitAngle(playerTransform, NewSplitAngle, ActualTransitionTime);
        }
    }
}