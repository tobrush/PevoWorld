using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Continuously lerps float value from defaultValue to targetValue over transitionTime.
/// </summary>
public class TimedTransition
{
    public bool TransitionComplete { get; private set; } = false;

    private float defaultValue = 0;
    private float originalValue = 0;
    private float currentValue = 0;
    private float targetValue = 0;
    private float transitionTime = 0;
    private float transitionTimeLeft = 0;

    public float CurrentValue {
        get
        {
            return currentValue == 0 ? defaultValue : currentValue;
        }
    }

    public Action OnTransitionComplete;
    public Action<float> OnCurrentValueChanged;

    /// <summary>
    /// Initializes timed transition with start value.
    /// </summary>
    /// <param name="defaultValue"></param>
    public TimedTransition(float defaultValue)
    {
        this.defaultValue = defaultValue;
    }

    /// <summary>
    /// Starts timed transition from defaultValue to targetValue over transitionTime.
    /// </summary>
    /// <param name="targetValue"></param>
    /// <param name="transitionTime"></param>
    public void StartTransition(float targetValue, float transitionTime)
    {
        originalValue = CurrentValue;
        this.targetValue = targetValue;
        this.transitionTime = transitionTime;
        transitionTimeLeft = transitionTime;

        TimedTransitionManager.StartTransition(BeginTransition);
    }

    private IEnumerator BeginTransition()
    {
        while (transitionTimeLeft > 0)
        {
            currentValue = Mathf.Lerp(originalValue, targetValue, 1 - (transitionTimeLeft / transitionTime));
            transitionTimeLeft -= Time.deltaTime;
            OnCurrentValueChanged?.Invoke(currentValue);
            yield return null;
        }

        currentValue = targetValue;
        transitionTimeLeft = 0;
        TransitionComplete = true;
        OnCurrentValueChanged?.Invoke(currentValue);
        OnTransitionComplete?.Invoke();
    }
}
