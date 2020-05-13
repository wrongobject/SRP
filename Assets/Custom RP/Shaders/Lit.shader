Shader "Custom RP/Lit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_BaseColor("BaseColor",Color) = (0.5,0.5,0.5,1)
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("SrcBlend",float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("DstBlend",float) = 0
		[Enum(On,1,Off,0)]_Zwrite("Zwrite",float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		Blend [_SrcBlend][_DstBlend]
		ZWrite [_Zwrite]
        Pass
        {
			Tags {
				"LightMode" = "CustomLight"
			}

			HLSLPROGRAM
			#pragma target 3.5
			#pragma vertex LitPassVertex
			#pragma fragment LitPassFragment
			#include "LitPass.hlsl"
			
			

			ENDHLSL
        }

		Pass {
			Tags {
				"LightMode" = "ShadowCaster"
			}

			ColorMask 0

			HLSLPROGRAM
			#pragma target 3.5
			#pragma shader_feature _CLIPPING
			#pragma multi_compile_instancing
			#pragma vertex ShadowCasterPassVertex
			#pragma fragment ShadowCasterPassFragment
			#include "ShadowCasterPass.hlsl"
			ENDHLSL
		}
    }
}
