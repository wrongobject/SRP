#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer renderer = new CameraRenderer();
    private ShadowSetting shadowSettings;
    public CustomRenderPipeline(bool useScriptableRenderPipelineBatching,bool lightsUseLinearIntensity, ShadowSetting shadowSettings)
    {                
        GraphicsSettings.useScriptableRenderPipelineBatching = useScriptableRenderPipelineBatching;
        GraphicsSettings.lightsUseLinearIntensity = lightsUseLinearIntensity;
        this.shadowSettings = shadowSettings;
    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            renderer.Render(context,camera, shadowSettings);
        }
    }
}
