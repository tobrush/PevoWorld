using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FadeManager
{
    private static Dictionary<Camera, Image> fadeImageMap = new Dictionary<Camera, Image>();
    private static Dictionary<Camera, TimedTransition> transitionMap = new Dictionary<Camera, TimedTransition>();
   
    /// <summary>
    /// Registers fadeImage with given camera.
    /// </summary>
    /// <param name="parentCamera"></param>
    /// <param name="fadeImage"></param>
    public static void RegisterFadeImage(Camera parentCamera, Image fadeImage)
    {
        fadeImageMap[parentCamera] = fadeImage;
    }

    /// <summary>
    /// Returns camera if it's enabled, otherwise finds active camera (assume at least main camera is enabled)
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Camera VerifyCameraEnabled(Camera camera)
    {
        if (!camera.enabled)
        {
            foreach (Camera c in fadeImageMap.Keys)
            {
                if (c.enabled)
                {
                    camera = c;
                }
            }
        }

        return camera;
    }

    /// <summary>
    /// Fades fadeImage of given camera from whatever color to fully transparent.
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="time"></param>
    /// <param name="OnFadeEnded"></param>
    public static void BeginFadeFromColor(Camera camera, float time, Action OnFadeEnded = null)
    {
        TimedTransition alphaTransition = new TimedTransition(1);
        alphaTransition.OnCurrentValueChanged = (currentValue) =>
        {
            Color oldColor = fadeImageMap[camera].color;
            fadeImageMap[camera].color = new Color(oldColor.r, oldColor.g, oldColor.b, currentValue);
        };
        transitionMap[camera] = alphaTransition;
        alphaTransition.StartTransition(0, time);

        if (OnFadeEnded != null)
        {
            alphaTransition.OnTransitionComplete = OnFadeEnded;
        }
    }

    /// <summary>
    /// Fades fadeImage of given camera from full transparent to color.
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="color"></param>
    /// <param name="time"></param>
    /// <param name="OnFadeEnded"></param>
    public static void BeginFadeToColor(Camera camera, Color color, float time, Action OnFadeEnded = null)
    {
        TimedTransition alphaTransition = new TimedTransition(0);
        alphaTransition.OnCurrentValueChanged = (currentValue) =>
        {
            Color oldColor = fadeImageMap[camera].color;
            fadeImageMap[camera].color = new Color(color.r, color.g, color.b, currentValue);
        };
        transitionMap[camera] = alphaTransition;
        alphaTransition.StartTransition(1, time);

        if (OnFadeEnded != null)
        {
            alphaTransition.OnTransitionComplete = OnFadeEnded;
        }
    }

    /// <summary>
    /// Fades all fadeImages from fully transparent to black.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="OnFadeEnded"></param>
    public static void BeginFadeAllToBlack(float time, Action OnFadeEnded = null)
    {
        List<Camera> cameras = new List<Camera>(fadeImageMap.Keys);
        for (int i = 0; i < cameras.Count; i++)
        {
            BeginFadeToColor(cameras[i], Color.black, time, i == 0 ? OnFadeEnded : null);
        }
    }

    /// <summary>
    /// Fades all fadeImages from fully transparent to white.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="OnFadeEnded"></param>
    public static void BeginFadeAllToWhite(float time, Action OnFadeEnded = null)
    {
        List<Camera> cameras = new List<Camera>(fadeImageMap.Keys);
        for (int i = 0; i < cameras.Count; i++)
        {
            BeginFadeToColor(cameras[i], Color.white, time, i == 0 ? OnFadeEnded : null);
        }
    }

    /// <summary>
    /// Fades all fadeImages from color to fully transparent.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="OnFadeEnded"></param>
    public static void BeginFadeAllFromColor(float time, Action OnFadeEnded = null)
    {
        List<Camera> cameras = new List<Camera>(fadeImageMap.Keys);
        for (int i = 0; i < cameras.Count; i++)
        {
            BeginFadeFromColor(cameras[i], time, i == 0 ? OnFadeEnded : null);
        }
    }
}
