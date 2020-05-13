#ifndef COMMON_INOUUT_STRUCT
#define COMMON_INOUUT_STRUCT

struct VetexInput{
	float3 positionOS : POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct FragInput{
	float4 postionHCS : SV_POSITION;	
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct BRDFVertex{
	float3 positionOS : POSITION;
	float3 normalOS : NORMAL;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};
struct BRDFFrag{
	float4 postionHCS : SV_POSITION;
	float3 positionWS :VAR_POSITION;
	float3 normalWS : NORMAL;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

#endif