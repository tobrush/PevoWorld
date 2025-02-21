using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for camera control ScriptableObjects.
/// </summary>
public class CameraControlBase : ScriptableObject
{
    [Header("General settings")]
    public bool InstantTransition = false;
    [Range(0.25f, 10)] public float TransitionTime = 1;
    
    protected float ActualTransitionTime
    {
        get { return InstantTransition ? 0 : TransitionTime; }
    }

    /// <summary>
    /// Called by CameraControlTrigger, all code should be inside overwriten functions in derived scripts.
    /// </summary>
    /// <param name="playerTransform"></param>
    public virtual void Activate(Transform playerTransform)
    {

    }
}
