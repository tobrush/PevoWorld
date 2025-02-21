using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public bool UnfadeOnStart = false;

    private void Awake()
    {
        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        while(!GetComponentInParent<Canvas>().worldCamera)
        {
            yield return null;
        }
        Camera c = GetComponentInParent<Canvas>().worldCamera;
        Image i = GetComponent<Image>();
        FadeManager.RegisterFadeImage(c, i);
        i.color = Color.black;

        if (UnfadeOnStart)
        {
            FadeManager.BeginFadeFromColor(c, 1);
        }
    }
}
