Shader "Unlit/TerrainPainter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coordinate ("Coordinate", Vector) = (0,0,0,0)
		_Scale("Scale", Vector) = (0,0,0,0)
		_Color ("Draw Color", Color) = (1,0,0,0)
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Coordinate, _Color, _Scale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float draw = pow(saturate(1- distance(i.uv, _Coordinate.xy)), 50);
				fixed4 drawCol = _Color * (draw * 100);				
				fixed4 x = saturate(col + drawCol);				
				// x -= 0.001f;
				return x;
			}
			ENDCG
		}
	}
}
