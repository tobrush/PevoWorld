using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTransitionManager : SingletonBehaviour<TimedTransitionManager>
{
    private static void CheckInstance()
    {
        if (Instance == null)
        {
            GameObject instance = new GameObject();
            instance.name = "Generic Transition Manager";
            TimedTransitionManager gtm = instance.AddComponent<TimedTransitionManager>();
            gtm.Awake();
        }
    }

    protected override void Awake()
    {
        if (Instance == null)
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Starts <paramref name="transitionCoroutine"/>. Should only be used by <see cref="TimedTransition"/>
    /// </summary>
    /// <param name="transitionCoroutine"></param>
    public static void StartTransition(Func<IEnumerator> transitionCoroutine)
    {
        CheckInstance();
        Instance.StartCoroutine(transitionCoroutine());
    }
}
