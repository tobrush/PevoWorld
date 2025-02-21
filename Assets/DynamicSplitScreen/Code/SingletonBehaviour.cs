using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Debug.LogWarning("Tried to initialize another instance of " + typeof(T).ToString());
            enabled = false;
        }
    }
}
