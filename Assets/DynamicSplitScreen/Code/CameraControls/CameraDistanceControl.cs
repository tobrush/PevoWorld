using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Camera Distance", menuName = "Camera Control/Camera Distance")]
public class CameraDistanceControl : CameraControlBase
{
    [Header("Controls")]
    public bool ApplyToMainCamera = false;
    public bool ApplyToPlayerCamera = false;
    [Range(0, 50)] public float Distance = 15;

    public override void Activate(Transform playerTransform)
    {
        if (ApplyToPlayerCamera)
        {
            SplitScreenManager.Instance.SetPlayerCameraDistance(playerTransform, Distance, ActualTransitionTime);
        }
        if (ApplyToMainCamera)
        {
            SplitScreenManager.Instance.SetMainCameraDistance(Distance, ActualTransitionTime);
        }
    }
}
