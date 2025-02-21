using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Camera Ortho Size", menuName = "Camera Control/Camera Ortho Size")]
public class CameraOrthoSizeControl : CameraControlBase
{
    [Header("Controls")]
    public bool ApplyToMainCamera = false;
    public bool ApplyToPlayerCamera = false;
    public float OrthoSize = 15;

    public override void Activate(Transform playerTransform)
    {
        if (ApplyToPlayerCamera)
        {
            SplitScreenManager.Instance.SetPlayerCameraOrthoSize(playerTransform, OrthoSize, ActualTransitionTime);
        }
        if (ApplyToMainCamera)
        {
            SplitScreenManager.Instance.SetMainCameraOrthoSize(OrthoSize, ActualTransitionTime);
        }
    }
}
