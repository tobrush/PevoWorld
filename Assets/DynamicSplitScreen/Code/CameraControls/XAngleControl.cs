using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera X Angle", menuName = "Camera Control/Camera X Angle")]
public class XAngleControl : CameraControlBase
{
    [Header("Controls")]
    [Range(45, 90)] public float CameraXAngle = 90;

    public override void Activate(Transform playerTransform)
    {
        SplitScreenManager.Instance.SetCameraXAngle(CameraXAngle, ActualTransitionTime);
    }
}