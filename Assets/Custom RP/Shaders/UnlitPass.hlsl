#ifndef CUSTOM_RP_UNLIT_PASS
#define CUSTOM_RP_UNLIT_PASS

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "UnityInput.hlsl"
#include "CommonInOutStruct.hlsl"

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float4,_MainTex_ST)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
	UNITY_DEFINE_INSTANCED_PROP(float,_CutOff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)


FragInput UnlitVert(VetexInput input)
{
	FragInput output;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input,output);
	float3 world_pos = float3(TransformObjectToWorld(input.positionOS));
	output.postionHCS = TransformWorldToHClip(world_pos);
	float4 uvst = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_MainTex_ST);
	output.uv = input.uv * uvst.xy + uvst.zw;
	return output;
}

float4  UnlitFrag(FragInput input) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(input);
	float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
	float4 texColor = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,input.uv);

	float4 result = texColor * baseColor;
#if defined _Clipping
	float cutoff = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_CutOff);
	clip(result.a - cutoff);
#endif
	return result;
}


#endif