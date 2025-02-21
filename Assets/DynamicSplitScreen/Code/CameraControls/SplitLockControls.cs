using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Split Lock", menuName = "Camera Control/Split Lock")]
public class SplitLockControls : CameraControlBase
{
    [Header("Controls")]
    public bool LockSplitScreen = false;
    public bool LockSplitAngle = false;

    public override void Activate(Transform playerTransform)
    {
        SplitScreenManager.Instance.LockSplitAngle = LockSplitAngle;
        SplitScreenManager.Instance.LockSplitScreen = LockSplitScreen;
    }
}