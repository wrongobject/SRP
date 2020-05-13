using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CustomRenderPipline")]
public class CustomRenderPiplineAsset : RenderPipelineAsset
{
    public bool lightsUseLinearIntensity = true;
    public bool useScriptableRenderPipelineBatching = true;
    [SerializeField]
    public ShadowSetting shadows = default;
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(useScriptableRenderPipelineBatching, lightsUseLinearIntensity,shadows);
    }
}
