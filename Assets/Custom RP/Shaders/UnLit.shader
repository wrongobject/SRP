Shader "Custom RP/Unlit"
{
    Properties
    {
		_MainTex("MainTex",2D)="white" {}
        _BaseColor("Color",Color)=(1,1,1,1)
		[Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("SrcBlend",float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("DstBlend",float) = 0
		[Enum(Off,0,On,1)]_ZWrite("ZWrite",float) = 1
		_CutOff("CutOff",Range(0,1)) = 1
		//[Enum(Off,0,On,1)]_Clipping("Clipping",float) = 0
		[Toggle(_Clipping)]_Clipping("Clipping",float) = 0
    }
    SubShader
    {
       
        Pass
        {
			
			Blend [_SrcBlend] [_DstBlend]
			ZWrite[_ZWrite]
			HLSLPROGRAM			

			#pragma shader_feature _Clipping
			#pragma multi_compile_instancing
			#pragma vertex UnlitVert
			#pragma fragment UnlitFrag
			#include "UnlitPass.hlsl"

			ENDHLSL
        }
    }
}
