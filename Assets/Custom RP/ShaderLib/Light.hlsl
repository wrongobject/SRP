#ifndef CUSTOM_LIGHT_INCLUDE
#define CUSTOM_LIGHT_INCLUDE

struct Light{
	float3 color;
	float3 dir;
	float attenuation;
};

#define MAX_DIRECTION_LIGHT_COUNT 4

CBUFFER_START(CustomLight)
	int _DirectionLightCount;
	float4 _DirectionLightColor[MAX_DIRECTION_LIGHT_COUNT];
	float4 _DirectionLightDirection[MAX_DIRECTION_LIGHT_COUNT];
	float4 _DirectionalLightShadowData[MAX_DIRECTION_LIGHT_COUNT];
CBUFFER_END

int GetLightCount(){
	return _DirectionLightCount;
}

DirectionalShadowData GetDirectionalShadowData (int lightIndex) {
	DirectionalShadowData data;
	data.strength = _DirectionalLightShadowData[lightIndex].x;
	data.tileIndex = _DirectionalLightShadowData[lightIndex].y;
	return data;
}

Light GetLight(int index, Surface surfaceWS){
	Light light;
	light.color = _DirectionLightColor[index].rgb;
	light.dir =  _DirectionLightDirection[index].xyz;
	DirectionalShadowData shadowData = GetDirectionalShadowData(index);
	light.attenuation = GetDirectionalShadowAttenuation(shadowData, surfaceWS);
	return light;
}


#endif