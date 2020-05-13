using UnityEngine.Rendering;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
public class Lighting
{
    const int MAX_DIRECTIONAL_LIGHT_COUNT = 4;
    const string bufferName = "Lighting";
    static int 
        dirLightColorId = Shader.PropertyToID("_DirectionLightColor"),      
        dirLightDirectionId = Shader.PropertyToID("_DirectionLightDirection"),    
        dirLightCountId = Shader.PropertyToID("_DirectionLightCount"),
        dirLightShadowDataId = Shader.PropertyToID("_DirectionalLightShadowData");

    static Vector4[] 
        colorArray = new Vector4[MAX_DIRECTIONAL_LIGHT_COUNT],
        dirArray = new Vector4[MAX_DIRECTIONAL_LIGHT_COUNT],
        dirLightShadowData = new Vector4[MAX_DIRECTIONAL_LIGHT_COUNT];

    CommandBuffer buffer = new CommandBuffer()
    {
        name = bufferName
    };
    Shadow shadows = new Shadow();
    public void Setup(ScriptableRenderContext context,ref CullingResults results, ShadowSetting shadowSettings)
    {
        buffer.BeginSample(bufferName);
        shadows.Setup(context, results, shadowSettings);
        NativeArray<VisibleLight> lights = results.visibleLights;
       
        int directionalLightCount = 0;
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].lightType != LightType.Directional)
                continue;           
            VisibleLight light = lights[i];
            SetLight(ref light, directionalLightCount);           
            directionalLightCount++;            
            if (directionalLightCount >= MAX_DIRECTIONAL_LIGHT_COUNT)
                break;
        }
        shadows.Render();

        buffer.SetGlobalVectorArray(dirLightShadowDataId, dirLightShadowData);
        buffer.SetGlobalFloat(dirLightCountId, directionalLightCount);
        buffer.SetGlobalVectorArray(dirLightColorId,colorArray);
        buffer.SetGlobalVectorArray(dirLightDirectionId, dirArray);
       

        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    private void SetLight(ref VisibleLight lit,int index)
    {
        Light light = lit.light;
        colorArray[index] = (light.color.linear * light.intensity);
        dirArray[index] = (-light.transform.forward);
        dirLightShadowData[index] = shadows.ReserveDirectionalShadows(lit.light, index);

    }

    public void Cleanup()
    {
        shadows.Cleanup();
    }
}

