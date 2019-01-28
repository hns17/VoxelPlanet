Shader "VoxelPlanet/tiles" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_OceanHeight("Ocean Height", Range(0,64)) = 0.0
		_OceanAtt("Light Attenuation", Range(0,20)) = 1.0
	}
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
	
		CGPROGRAM
		#pragma surface surf Standard
		#pragma target 3.0
	
		sampler2D _MainTex;
	
		fixed _OceanAtt;
		fixed _OceanHeight;
	
		struct Input {
			float2 uv_MainTex;
			half3 worldPos;
		};
	
		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)
	
		void surf(Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
	
			//수면에 의한 감쇄
			half dist = length(IN.worldPos);
			if (dist < _OceanHeight) 
				o.Albedo *= saturate(pow((dist / _OceanHeight), _OceanAtt));
			
		}
	
		ENDCG
	}
	FallBack "Diffuse"
}
