using DynamicSplitScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates specified controls in OnTriggerEnter / OnTriggerExit.
/// </summary>
public class CameraControlTrigger : MonoBehaviour
{
    private bool is3D = false;

    [Header("Controls")]
    public bool OneShot = false;
    public CameraControlBase[] onTriggerEnterControls;
    public CameraControlBase[] onTriggerExitControls;

    [Header("Fade")]
    public bool FadeOutBeforeTransition = false;
    public bool FadeInAfterTransition = false;
    [Range(0, 10)] public float FadeOutDuration = 0.2f;
    [Range(0, 10)] public float FadeInDuration = 0.2f;

    private void Start()
    {
        is3D = GetComponent<Collider2D>() == null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform hit = other.transform;
        if (!SplitScreenManager.Instance.IsTransformRegisteredPlayer(hit) || onTriggerEnterControls.Length == 0) return;

        ActivateEffectsWithFade(hit, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform hit = other.transform;
        if (!SplitScreenManager.Instance.IsTransformRegisteredPlayer(hit) || onTriggerEnterControls.Length == 0) return;

        ActivateEffectsWithFade(hit, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Transform hit = other.transform;
        if (!SplitScreenManager.Instance.IsTransformRegisteredPlayer(hit) || onTriggerExitControls.Length == 0) return;

        ActivateEffectsWithFade(hit, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform hit = other.transform;
        if (!SplitScreenManager.Instance.IsTransformRegisteredPlayer(hit) || onTriggerExitControls.Length == 0) return;

        ActivateEffectsWithFade(hit, false);
    }

    private void ActivateEffectsWithFade(Transform hit, bool enter)
    {
        if (!enabled) return;

        if (FadeOutBeforeTransition)
        {
            FadeManager.BeginFadeAllToBlack(FadeOutDuration, () =>
            {
                ActivateEffects(hit, enter);
            });
        }
        else
        {
            ActivateEffects(hit, enter);
        }

        if (OneShot)
        {
            enabled = false;
        }
    }

    private void FadeIn()
    {
        if (FadeInAfterTransition)
        {
            FadeManager.BeginFadeAllFromColor(FadeInDuration);
        }
    }

    private void ActivateEffects(Transform hit, bool enter)
    {
        foreach (CameraControlBase c in enter ? onTriggerEnterControls : onTriggerExitControls)
        {
            c.Activate(hit);
        }

        FadeIn();
    }

#if UNITY_EDITOR
    [Header("Gizmos")]
    public Color TriggerColor = Color.black;

    private void OnValidate()
    {
        is3D = GetComponent<Collider2D>() == null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = TriggerColor;

        if (is3D) Gizmos.DrawWireCube(transform.position, transform.localScale);
        else Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
#endif
}
