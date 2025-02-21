//------------------------------------
//             OmniShade
//     Copyright© 2022 OmniShade     
//------------------------------------

#include "OmniShadeURP.cginc"

//////////////////////////////////////////////////////////////////////////////////////////
// PROPERTY DECLARATIONS
//////////////////////////////////////////////////////////////////////////////////////////
CBUFFER_START(UnityPerMaterial)
sampler2D _MainTex;
half4 _MainTex_ST;
half4 _Color;
half _Brightness;
half _Contrast;
half _Saturation;
half _IgnoreMainTexAlpha;

sampler2D _MatCapTex;
half4 _MatCapTex_ST;
half4 _MatCapColor;
half _MatCapBrightness;
half _MatCapContrast;
half3 _MatCapRot;

sampler2D _NormalTex;
half4 _NormalTex_ST;
half _NormalStrength;

half _AmbientBrightness;

half4 _ShadowColor;

half _ZOffset;
CBUFFER_END

//////////////////////////////////////////////////////////////////////////////////////////
// VERTEX STRUCTURE OUTPUT
//////////////////////////////////////////////////////////////////////////////////////////
struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 pos_world : TEXCOORD1;
	float3 nor_world : TEXCOORD2;
	#if MATCAP
		#if !NORMAL_MAP
			float3 nor_view : TEXCOORD3;
		#elif MATCAP_PERSPECTIVE
			float3 viewDir_view : TEXCOORD3;
		#endif
	#endif
	#if NORMAL_MAP && MATCAP
		float4 tan_world : TEXCOORD4;
	#endif
	#if FOG
		float fogCoord : TEXCOORD5;
	#endif
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING) && SHADOWS_ENABLED
		UNITY_SHADOW_COORDS(6)
	#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO 
};

//////////////////////////////////////////////////////////////////////////////////////////
// FUNCTIONS
//////////////////////////////////////////////////////////////////////////////////////////
half4 ColorBrightness(half4 base, half brightness, half4 color) {
	base.rgb *= brightness;
	return base * color;
}

float3 TangentSpaceNormalToWorldSpaceNormal(float3 nor_world, float4 tan_world, float3 nor_tan) {
	float3 bin_world = cross(nor_world, tan_world.xyz) * tan_world.w;
	float3 ts0 = float3(tan_world.x, bin_world.x, nor_world.x);
	float3 ts1 = float3(tan_world.y, bin_world.y, nor_world.y);
	float3 ts2 = float3(tan_world.z, bin_world.z, nor_world.z);
	return normalize(float3(dot(ts0, nor_tan), dot(ts1, nor_tan), dot(ts2, nor_tan)));
}

half3 BlendMatCap(half3 col, half3 col_blend) {
	#if _MATCAPBLEND_MULTIPLY
		col *= col_blend;
	#elif _MATCAPBLEND_MULTIPLY_LIGHTEN
		col *= 1 + col_blend;
	#endif
	return col;
}

//////////////////////////////////////////////////////////////////////////////////////////
// VERTEX SHADER
//////////////////////////////////////////////////////////////////////////////////////////
v2f vert (appdata_full v) {
	v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o)
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 

	o.pos_world = mul(unity_ObjectToWorld, v.vertex).xyz;

	// Clip pos
	#if ZOFFSET
		o.pos.xyz = UnityObjectToViewPos(v.vertex.xyz);
		o.pos.z += _ZOffset;
		o.pos = mul(UNITY_MATRIX_P, float4(o.pos.xyz, 1));
	#else
		o.pos = UnityObjectToClipPos(v.vertex.xyz);
	#endif

	// Fragment data
	o.uv = v.texcoord.xy;
	o.nor_world = UnityObjectToWorldNormal(v.normal);
	#if NORMAL_MAP && MATCAP
		o.tan_world = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
	#endif
	#if MATCAP
		#if !NORMAL_MAP
			float3 worldNorm = UnityObjectToWorldNormal(v.normal);
			float3x3 viewMat = ((float3x3)UNITY_MATRIX_V);
			#if MATCAP_STATIC
				// Remove view rotation
				viewMat[0] = float3(1, 0, 0);
				viewMat[1] = float3(0, 1, 0);
				viewMat[2] = float3(0, 0, 1);
				// Add custom direction rotation
				float3 cosRot = cos(_MatCapRot);
				float3 sinRot = sin(_MatCapRot);
				float3x3 rotX = float3x3(1, 0, 0, 0, cosRot.x, -sinRot.x, 0, sinRot.x, cosRot.x);
				float3x3 rotY = float3x3(cosRot.y, 0, sinRot.y,	0, 1, 0, -sinRot.y, 0, cosRot.y);
				float3x3 rotZ = float3x3(cosRot.z, -sinRot.z, 0, sinRot.z, cosRot.z, 0, 0, 0, 1);
				worldNorm = mul(rotX, mul(rotY, mul(rotZ, worldNorm)));
				o.nor_view = mul(viewMat, worldNorm);
			#else  
				o.nor_view = mul(viewMat, worldNorm);
                #if MATCAP_PERSPECTIVE
					float3 viewDir_view = normalize(UnityObjectToViewPos(v.vertex.xyz));
					float3 viewCross = cross(viewDir_view, o.nor_view);
					o.nor_view = float3(-viewCross.y, viewCross.x, 0.0);
				#endif
			#endif
		#elif MATCAP_PERSPECTIVE
			o.viewDir_view = normalize(UnityObjectToViewPos(v.vertex.xyz));
		#endif
	#endif

	// Unity system
	#if FOG
		UNITY_TRANSFER_FOG(o, o.pos);
	#endif
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING) && SHADOWS_ENABLED
		UNITY_TRANSFER_SHADOW(o, o.uv.zw);
	#endif
	#if SHADOW_CASTER
		TRANSFER_SHADOW_CAST(o, v)
	#endif

	return o;
}

//////////////////////////////////////////////////////////////////////////////////////////
// FRAGMENT SHADER
//////////////////////////////////////////////////////////////////////////////////////////
half4 frag (v2f i) : COLOR {
	float2 uv = i.uv;

	#if CUTOUT
		float shadowAlpha = tex2D(_MainTex, TRANSFORM_TEX(uv, _MainTex)).a;
		if (shadowAlpha < 0.5)
			discard;
	#endif
	#if SHADOW_CASTER
		return 0;
	#endif	
	UNITY_SETUP_INSTANCE_ID(i);

	// Normalize vectors
	i.nor_world = normalize(i.nor_world);
	#if NORMAL_MAP && MATCAP
			i.tan_world.xyz = normalize(i.tan_world.xyz);
	#endif
	#if MATCAP && MATCAP_PERSPECTIVE && NORMAL_MAP
		i.viewDir_view = normalize(i.viewDir_view);
	#endif

	// Calculate intermediate vectors
	#if NORMAL_MAP	// Calculate tangent-space normal
		float2 normalUV = uv;
		float3 nor_tan = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(normalUV, _NormalTex)));
		nor_tan.xy *= _NormalStrength;
		nor_tan = normalize(nor_tan);
	#else
		float3 nor_tan = float3(0, 0, 1);
	#endif
	#if NORMAL_MAP && MATCAP
		float3 bumpNor_world = TangentSpaceNormalToWorldSpaceNormal(i.nor_world, i.tan_world, nor_tan);
	#endif
	#if MATCAP
		#if NORMAL_MAP
			float3 adjNor_world = bumpNor_world;
		#else  // Not using normal map - just use nor_world from vertex shader
			float3 adjNor_world = i.nor_world;
		#endif
	#endif

	// BASE
	float4 col_base = tex2D(_MainTex, TRANSFORM_TEX(uv, _MainTex));
	col_base.a = _IgnoreMainTexAlpha == 0 ? saturate(col_base.a) : 1;
	#if BASE_CONTRAST
		col_base.rgb = pow(col_base.rgb, _Contrast);
	#endif
	col_base = ColorBrightness(col_base, _Brightness, _Color);

	// LIGHTING
	half3 col = col_base.rgb;
	#if MATCAP  // Calculate view-space normal
		#if NORMAL_MAP
			float3 nor_view = normalize(mul((float3x3)UNITY_MATRIX_V, adjNor_world));
			#if MATCAP_PERSPECTIVE
				float3 viewCross = cross(i.viewDir_view, nor_view);
				nor_view = float3(-viewCross.y, viewCross.x, 0.0);
			#endif
		#else  // Use simplified matcap calculation without normal map
			float2 nor_view = i.nor_view.xy;
		#endif
		float2 matCapUV = nor_view.xy * 0.5 + 0.5;
		half3 col_matcap = tex2D(_MatCapTex, TRANSFORM_TEX(matCapUV, _MatCapTex)).rgb;
		#if MATCAP_CONTRAST
			col_matcap = pow(col_matcap, _MatCapContrast);
		#endif
		col_matcap = col_matcap * _MatCapColor.rgb * _MatCapBrightness;
		col = BlendMatCap(col.rgb, col_matcap);
	#endif
	
	// Shadows
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING || \
		_MAIN_LIGHT_SHADOWS || _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS_SCREEN) && SHADOWS_ENABLED
		UNITY_LIGHT_ATTENUATION(atten, i, i.pos_world)
		half3 col_shadow = lerp(_ShadowColor.rgb, 1, atten);
		col *= col_shadow;
	#endif

	// Additives
	#if AMBIENT
		half3 col_ambient = ShadeSH9(float4(i.nor_world, 1.0)).rgb * _AmbientBrightness;
		col += col_base.rgb * col_ambient;
	#endif
	#if BASE_SATURATION
		half3 col_desat = Luminance(col);
		col = lerp(col_desat, col, _Saturation);
	#endif

	// Overlays
	#if FOG
		UNITY_APPLY_FOG(i.fogCoord, col);
	#endif

	return half4(col, col_base.a);
}
