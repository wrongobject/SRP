#ifndef CUSTOM_LIGHTING_INCLUDE
#define CUSTOM_LIGHTING_INCLUDE

float3 DiffuseLight(Surface surf,Light light){
	
	return saturate(dot(surf.normal,light.dir)) * surf.color * light.color;
	//return saturate(dot(surf.normal,light.dir)) * surf.color * light.color;
}

float3 IncomingLight (Surface surface, Light light) {
	return saturate(dot(surface.normal, light.dir) * light.attenuation) * light.color;
}

float3 GetDirectBRDF (Surface surf, Light light) {
	BRDF brdf = GetBRDF(surf);
	return SpecularStrength(surf, brdf, light) * brdf.specular + brdf.diffuse;
}


float3 GetDiffuseLight(Surface surf)
{
	float3 result;
	for(int i = 0; i < GetLightCount();i++){
		Light light = GetLight(i,surf);
		result = result + DiffuseLight(surf,light);
	}
	return result;	
}

float3 GetBRDFLighting(Surface surf){
	float3 result;
	for(int i = 0; i < GetLightCount();i++){
		Light light = GetLight(i,surf);
		result = result + GetDirectBRDF(surf,light) * IncomingLight(surf,light);
	}
	return result;	
}

#endif