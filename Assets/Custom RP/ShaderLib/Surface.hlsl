#ifndef CUSTOM_SURFACE_INCLUDE
#define CUSTOM_SURFACE_INCLUDE

struct Surface{
	float3 color;
	float3 normal;
	float alpha;
	float3 diff;
	float3 specular;
	float smoothness;
	float metallic;
	float3 viewDir;	
	float3 position;
};

#endif