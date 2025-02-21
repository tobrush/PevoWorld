using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used only to render split screen render texture to screen. SplitRT is passed by SplitScreenManager.
/// </summary>
public class SplitWorldCamera : MonoBehaviour
{
    public RenderTexture SplitRT;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(SplitRT, destination);
    }
}
