Shader "VoxelPlanet/tiles_clouds" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Ambient("Ambient", Range(0, 10)) = 1.0

		_Distortion("Distortion", Range(-10,10)) = 1.0
		_Scale("TransScale", Range(0, 20)) = 1.0

		_Attenuation("Attenuation", Range(0.0, 2.0)) = 0.0
		_Thikness("Thikness", Range(0, 5)) = 1.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf StandardClouds fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Distortion, _Scale, _Ambient, _Attenuation, _Thikness;
		fixed4 _Color;

		#include "UnityPBSLighting.cginc"
		inline fixed4 LightingStandardClouds(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi) {
			fixed4 pbr = LightingStandard(s, viewDir, gi);

			// --- Translucency ---
			float3 L = gi.light.dir;
			float3 V = viewDir;
			float3 N = s.Normal;

			//내부산란에 의한 빛의 굴절률
			float3 H = normalize(L + N * _Distortion);

			//물체의 투명도
			float VdotH = saturate(dot(V, -H)) * _Scale;

			//깊이에 따른 빛의 감쇄
			float3 I = _Attenuation * (VdotH + _Ambient) *_Thikness;

			pbr.rgb = pbr.rgb + gi.light.color * I;

			return pbr;
		}

		void LightingStandardClouds_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
