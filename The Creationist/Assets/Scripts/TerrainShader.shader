// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/TerrainShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_SplatGrasslands ("SplatMapGrasslands", 2D) = "black" {}
		_SplatForest ("SplatMapForest", 2D) = "black" {}
		_SplatTundra ("SplatMapTundra", 2D) = "black" {}

		_Plains ("Plains (Default Albedo)", 2D) = "black" {}		
		_Grassland ("Grassland", 2D) = "black" {}
		_Forest ("Forest", 2D) = "black" {}
		_Tundra ("Tundra", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.6
		
		sampler2D _SplatGrasslands;
		sampler2D _SplatForest;
		sampler2D _SplatTundra;

		sampler2D _Plains;	
		
		sampler2D _Grassland;		
		sampler2D _Forest;		
		sampler2D _Tundra;		

		struct Input {			
			float2 uv_SplatGrasslands;
			float2 uv_SplatForest;
			float2 uv_SplatTundra;
			
			float2 uv_Plains;			
			float2 uv_Forest;	
			float2 uv_Tundra;
			float2 uv_Grassland;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color

			fixed3 colour;			

			colour = tex2D (_Plains, IN.uv_Plains).rgb;			
			
			colour += tex2D(_SplatGrasslands, IN.uv_SplatGrasslands).r * tex2D (_Grassland, IN.uv_Grassland).rgb;
			colour /= colour.x + colour.y + colour.z;
			colour += tex2D(_SplatForest, IN.uv_SplatForest).r * tex2D (_Forest, IN.uv_Forest).rgb;
			colour /= colour.x + colour.y + colour.z;
			colour += tex2D(_SplatTundra, IN.uv_SplatTundra).r * tex2D (_Tundra, IN.uv_Tundra).rgb;						
			colour /= colour.x + colour.y + colour.z;
			/*
			if(tex2D(_SplatGrasslands, IN.uv_SplatGrasslands).r >= tex2D(_SplatForest, IN.uv_SplatForest).r)
			{
				if(tex2D(_SplatGrasslands, IN.uv_SplatGrasslands).r >= tex2D(_SplatTundra, IN.uv_SplatTundra).r)
				{
					colour = tex2D(_SplatGrasslands, IN.uv_SplatGrasslands).r * tex2D (_Grassland, IN.uv_Grassland).rgb;
				}
				else
				{
					colour = tex2D (_Plains, IN.uv_Plains);
				}
			}
			else
			{
				colour = tex2D (_Plains, IN.uv_Plains);
			}*/

			o.Albedo = colour.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			//o.Alpha = colour.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
