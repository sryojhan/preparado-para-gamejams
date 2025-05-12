using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class SceneTransitionRendererFeature : ScriptableRendererFeature
{
    class SceneTransitionPass : ScriptableRenderPass
    {
        const string m_PassName = "SceneTransitionPass";
        Material m_BlitMaterial;

        public void Setup(Material mat)
        {
            m_BlitMaterial = mat;
            requiresIntermediateTexture = true;
        }


        // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
        // FrameData is a context container through which URP resources can be accessed and managed.
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();

            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError("Skipping render pass. Scene transition requires an intermediate ColorTexture, we cant use the BackBuffer as a texture input");
                return;
            }

            var source = resourceData.activeColorTexture;

            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = $"CameraColor-{m_PassName}";
            destinationDesc.clearBuffer = false;

            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);


            RenderGraphUtils.BlitMaterialParameters param = new RenderGraphUtils.BlitMaterialParameters(source, destination, m_BlitMaterial, 0);

            renderGraph.AddBlitPass(param, passName: m_PassName);

            resourceData.cameraColor = destination;
        }
    }

    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRendering;
    public Material material;

    SceneTransitionPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new SceneTransitionPass();

        // Configures where the render pass should be injected.
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        if(material == null)
        {
            Debug.LogWarning("Scene transition has no material applied");
            return;
        }

        m_ScriptablePass.Setup(material);
        m_ScriptablePass.renderPassEvent = injectionPoint;

        renderer.EnqueuePass(m_ScriptablePass);
    }
}
