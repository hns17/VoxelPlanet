Shader "VoxelPlanet/Water" {
	Properties {
		_MainColor("MainColor", Color) = (1,1,1,1)
		_NoiseTex ("Albedo (RGB)", 2D) = "white" {}
		_WaterDistortion ("WaterDistortion", RANGE(0.00, 0.1)) = 0.1
		_WaterFaceIntencity ("WaterFace Intencity", RANGE(0.00, 1.00)) = 0.1
		_LightTransmittance ("WaterLight Transmittance", RANGE(0.0, 2.0)) = 1.0
		_LightTransmittance2 ("WaterLight Transmittance2", RANGE(0.0, 0.1)) = 0.01
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		GrabPass{}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 4.0

		sampler2D _GrabTexture;
		sampler2D _CameraDepthTexture;

		sampler2D _NoiseTex;

		float4 _MainColor;

		float _WaterDistortion;
		float _WaterFaceIntencity;
		float _LightTransmittance;
		float _LightTransmittance2;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};


		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)



		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 d = tex2D(_NoiseTex, IN.uv_MainTex + _Time.x);
			
			//screenUV 가져오기
			float4 nScreen = IN.screenPos / (IN.screenPos.w + 0.000000001);
			float4 screenDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(
				tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(nScreen))
			));

			//물에서 빛의 굴절
			o.Emission = tex2D(_GrabTexture, nScreen.rg + d.r * _WaterDistortion).rgb;

			//깊이에 따른 빛의 투과율
			float3 depthLight = pow((saturate(screenDepth * _LightTransmittance2)).rgb, _LightTransmittance);
			
			//물의 깊이에 따른 지면 렌더
			o.Emission *= 1 - depthLight;

			//수면 색상 렌더, 비율은 색상의 a 값으로 조절
			o.Emission += depthLight * _MainColor.rgb * _MainColor.a;
			
			//오브젝트와 수면의 경계
			float3 intersect = saturate(1 - (screenDepth - LinearEyeDepth(nScreen.z))) * _WaterFaceIntencity;
			o.Emission += intersect * _MainColor.rgb;
			
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Legacy shaders/Transparent/Diffuse"
}
