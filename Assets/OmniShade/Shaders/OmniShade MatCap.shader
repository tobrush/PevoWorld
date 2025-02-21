//------------------------------------
//             OmniShade
//     Copyright© 2022 OmniShade     
//------------------------------------

Shader "OmniShade/MatCap" { 
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Brightness ("Brightness", range(0, 25)) = 1
		_Contrast ("Contrast", range(0, 25)) = 1
		_Saturation ("Saturation", range(0, 2)) = 1
		[Toggle] _IgnoreMainTexAlpha ("Ignore Main Texture Alpha", Float) = 0

		[HeaderGroup(MatCap)]
		_MatCapTex ("MatCap Texture", 2D) = "black" {}
		_MatCapColor ("MatCap Color", Color) = (1, 1, 1, 1)
		_MatCapBrightness ("MatCap Brightness", range(0, 25)) = 1
		_MatCapContrast ("MatCap Contrast", range(0, 25)) = 1
		[KeywordEnum(Multiply, Multiply Lighten)] _MatCapBlend ("MatCap Blend Mode", Float) = 0
        [Toggle(MATCAP_PERSPECTIVE)] _MatCapPerspective ("Perspective Correction", Float) = 1
		[Toggle(MATCAP_STATIC)] _MatCapStatic ("Use Static Rotation", Float) = 0
		_MatCapRot ("MatCap Static Rotation", Vector) = (0, 0, 0, 0)

		[HeaderGroup(Normal Map)]
		[Normal] _NormalTex ("Normal Map", 2D) = "bump" {}
		_NormalStrength ("Normal Strength", range(0, 5)) = 1
		
		[HeaderGroup(Environment And Shadows)]
		_AmbientBrightness ("Ambient Brightness", range(0, 25)) = 1
		[Toggle(FOG)] _Fog ("Enable Fog", Float) = 1

		[Header(Shadows)]
		[Toggle(SHADOWS_ENABLED)] _ShadowsEnabled ("Enable Shadows", Float) = 1
		[HDR] _ShadowColor ("Shadow Color", Color) = (0.3, 0.3, 0.3, 1)
		
		[HeaderGroup(Culling And Blending)]
		[Enum(Opaque, 0, Transparent, 1, Transparent Additive, 2, Transparent Additive Alpha, 3, Opaque Cutout, 4)] _Preset ("Culling And Blend Preset", Float) = 0

		[Header(Culling)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Culling", Float) = 2 				// Back
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1.0								// On
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", Float) = 4 			// LessEqual
		_ZOffset ("Depth Offset", range(-5, 5)) = 0
		[Toggle(CUTOUT)] _Cutout ("Cutout Transparency", Float) = 0

		[Header(Blending)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SourceBlend ("Source Blend", Float) = 5 	// SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] _DestBlend ("Dest Blend", Float) = 10 		// OneMinusSrcAlpha
		[Enum(UnityEngine.Rendering.BlendOp)] _BlendOp ("Blend Mode", Float) = 0  			// Add		// Add
	}
	
	Subshader {
		Pass {
			Name "Forward"
			Tags { "LightMode" = "ForwardBase" }
			Cull [_Cull]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			Blend [_SourceBlend][_DestBlend]
			BlendOp [_BlendOp]

			CGPROGRAM
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"
			#include "OmniShadeCore.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_instancing
			#pragma multi_compile _ SHADOWS_SCREEN
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma shader_feature BASE_CONTRAST
			#pragma shader_feature BASE_SATURATION
			#pragma shader_feature MATCAP
			#pragma shader_feature MATCAP_CONTRAST
			#pragma shader_feature _MATCAPBLEND_MULTIPLY _MATCAPBLEND_MULTIPLY_LIGHTEN
			#pragma shader_feature MATCAP_PERSPECTIVE
			#pragma shader_feature MATCAP_STATIC
			#pragma shader_feature NORMAL_MAP
			#pragma shader_feature AMBIENT
			#pragma shader_feature FOG
			#pragma shader_feature SHADOWS_ENABLED
			#pragma shader_feature ZOFFSET
			#pragma shader_feature CUTOUT
			ENDCG
		}

		Pass {
			name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			Cull [_Cull]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			Blend [_SourceBlend][_DestBlend]
			BlendOp [_BlendOp]

			CGPROGRAM
			#define SHADOW_CASTER 1
			#include "UnityCG.cginc"
			#include "OmniShadeCore.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma shader_feature CUTOUT
			ENDCG
		}
	}

	CustomEditor "OmniShadeGUI"
}
