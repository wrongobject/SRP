#ifndef CUSTOM_LIT_PASS_INCLUDED
#define CUSTOM_LIT_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "UnityInput.hlsl"
#include  "CommonInOutStruct.hlsl"
#include "../ShaderLib/Surface.hlsl"
#include "../ShaderLib/Shadows.hlsl"
#include "../ShaderLib/Light.hlsl"
#include "../ShaderLib/BRDF.hlsl"
#include "../ShaderLib/Lighting.hlsl"

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float4,_MainTex_ST)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
	UNITY_DEFINE_INSTANCED_PROP(float, _CutOff)
	UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
	UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

BRDFFrag LitPassVertex (BRDFVertex input) {
	BRDFFrag output;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input,output);
	float4 mainSt = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_MainTex_ST);
	output.uv = input.uv * mainSt.xy + mainSt.zw;

	float3 world_pos = float3(TransformObjectToWorld(input.positionOS));
	output.postionHCS = TransformWorldToHClip(world_pos);
	output.positionWS = world_pos;
	output.normalWS = TransformObjectToWorldNormal(input.normalOS);

	return output;
}	

float4 LitPassFragment (BRDFFrag input) : SV_TARGET {
	UNITY_SETUP_INSTANCE_ID(input);
	float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
	float4 texColor = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,input.uv);
	Surface surf;
	surf.normal = input.normalWS;
	surf.color = baseColor.rgb * texColor.rgb;
	surf.alpha = texColor.a;
	surf.metallic = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Metallic);
	surf.smoothness = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Smoothness);
	surf.viewDir = normalize(_WorldSpaceCameraPos - input.positionWS);
	surf.position = input.positionWS;
	float4 result = float4(GetBRDFLighting(surf),surf.alpha);
	return result;
}

#endif