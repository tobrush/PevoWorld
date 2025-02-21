// Defines for rendering pipeline - if LWRP, URP and HDRP are false, standard rendering will be used
//#define URP
//#define HDRP
//#define LWRP

using UnityEngine;
using UnityEngine.Rendering;

#if LWRP
using RP = UnityEngine.Rendering.LWRP;
#elif URP
using RP = UnityEngine.Rendering.Universal;
#elif HDRP
using RP = UnityEngine.Rendering.HighDefinition;
#endif

#if LWRP || URP
public class SplitScreenRendererPass : RP.ScriptableRendererFeature
{
    class CustomRenderPass : RP.ScriptableRenderPass
    {
        public bool MainCameraOnly = true;

        public RenderTexture splitRT;

        private const string cameraRT = "_CameraColorTexture";

        public const string topLayerShader = "Top Layer Shader";
        public Material topLayerMaterial;

        public override void Execute(ScriptableRenderContext context, ref RP.RenderingData renderingData)
        {
            if (MainCameraOnly) return;

            CommandBuffer buf = new CommandBuffer();
            buf.Blit(splitRT, cameraRT); // copy splitRT into camera target texture
            context.ExecuteCommandBuffer(buf);
        }
    }

    CustomRenderPass m_ScriptablePass;

    /// <summary>
    /// Sets mode of the renderer (main camera only or split screen)
    /// </summary>
    /// <param name="mainCameraOnly"></param>
    public void SetMode (bool mainCameraOnly, RenderTexture splitRT = null)
    {
        if (m_ScriptablePass.MainCameraOnly == mainCameraOnly) return;

        m_ScriptablePass.MainCameraOnly = mainCameraOnly;
        m_ScriptablePass.splitRT = splitRT;
    }

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();

        m_ScriptablePass.topLayerMaterial = CoreUtils.CreateEngineMaterial(Shader.Find(CustomRenderPass.topLayerShader));

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RP.RenderPassEvent.BeforeRendering;
    }
    
    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(RP.ScriptableRenderer renderer, ref RP.RenderingData renderingData)
    {
        if (renderingData.cameraData.camera != Camera.main) return;

        renderer.EnqueuePass(m_ScriptablePass);
    }
}

#elif HDRP

//public class SplitScreenRendererPass : CustomPass

#endif