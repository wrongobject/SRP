#ifndef CUSTOM_BRDF_INCLUDE
#define CUSTOM_BRDF_INCLUDE

#define MAX_MATALLIC 0.96

struct BRDF{
	float3 diffuse;
	float3 specular;
	float roughness;
};

float Square(float v)
{
	return v * v;
}

float SpecularStrength (Surface surface, BRDF brdf, Light light) {
	float3 h = SafeNormalize(light.dir + surface.viewDir);
	float nh2 = Square(saturate(dot(surface.normal, h)));
	float lh2 = Square(saturate(dot(light.dir, h)));
	float r2 = Square(brdf.roughness);
	float d2 = Square(nh2 * (r2 - 1.0) + 1.00001);
	float normalization = brdf.roughness * 4.0 + 2.0;
	return r2 / (d2 * max(0.1, lh2) * normalization);
}



float OneMinusReflectivity (float metallic) {
	//metallic 越大，自己的颜色属性影响越低	
	//控制matallic最大值小于1;
	return MAX_MATALLIC - metallic * MAX_MATALLIC;
}

BRDF GetBRDF(Surface surf)
{
	BRDF brdf;		
	float oneMinusReflectivity = OneMinusReflectivity(surf.metallic);
	brdf.diffuse = surf.color * oneMinusReflectivity;
	//另外一部分光进行反射
	brdf.specular = lerp( 1 - MAX_MATALLIC,surf.color,surf.metallic);
	float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surf.smoothness);		
	brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
	return brdf;
}

#endif