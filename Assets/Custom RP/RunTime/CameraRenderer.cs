using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public partial class CameraRenderer 
{
    #region const & static value
    private const string bufferName = "Render Camera";

    private static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM"),
        new ShaderTagId("CustomLight"),
    };
    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    private static Material errorMaterial;
    #endregion

    private ScriptableRenderContext context;
    private Camera camera;
    private CullingResults cullingResults;
    private CommandBuffer buffer;
    private Lighting light = new Lighting();    
    public CameraRenderer()
    {        
        buffer = new CommandBuffer()
        {
            name = bufferName
        };
    }

    public void Render(ScriptableRenderContext context, Camera camera, ShadowSetting shadowSettings)
    {
        this.context = context;
        this.camera = camera;
#if UNITY_EDITOR
        PrepareForBuffer();
        PrepareForSceneWindow();
#endif

        if (!Cull(shadowSettings.maxDistance))
        {
            return;
        }
        buffer.BeginSample(bufferName);
        ExecuteBuffer();
        light.Setup(context,ref cullingResults, shadowSettings);
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        Setup();
        DrawVisibleGeometry();
#if UNITY_EDITOR

        DrawUnsupportedShaders();
        DrawGizmos();

#endif
        light.Cleanup();
        Submit();
    }
    
  
    private void Setup()
    {
        context.SetupCameraProperties(camera);

        CameraClearFlags flags = camera.clearFlags;

        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth,
            flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear
            );

        buffer.BeginSample(bufferName);
        ExecuteBuffer();
       
    }
    private void DrawVisibleGeometry()
    {
        SortingSettings sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId,sortingSettings);
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
              
        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );
        context.DrawSkybox(camera);
       
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );
    }

    private void Submit()
    {
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        context.Submit();
    }  

    private void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);        
        buffer.Clear();
    }

    private bool Cull(float maxShadowDistance)
    {       
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
            cullingResults = context.Cull(ref p);
            return true;
        }
        return false;
    }

    
}
