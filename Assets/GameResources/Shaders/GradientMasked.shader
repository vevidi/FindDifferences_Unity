﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GradientMasked" 
{ 
	Properties 
	{ 
		_MainTex ("Texture", 2D) = "white" {} 
		 _AlphaTex ("External Alpha", 2D) = "white" {}
		_Color1 ("First Color", Color) = (1,1,1,1) 
		_Color2 ("Second Color", Color) = (1,1,1,1) 
		_Height ("Height", Float) = 10.0 
	} 
	
	SubShader 
	{ 
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjectors"="True" } 
		Cull Off
		LOD 100 
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		
			#include "UnityCG.cginc"

            struct appdata_t 
			{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _AlphaTex;  
			float4 _MainTex_ST;
			float4 _AlphaTex_ST;
			float4 _Color1;
			float4 _Color2;
			float _Height;
		
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
		
			half4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);	
				fixed4 texcol_a = tex2D(_AlphaTex, i.uv);
				fixed4 gradient = lerp(_Color1, _Color2, i.worldPos.y / _Height);			
				col = col * gradient * texcol_a;
				return col;
			}
			ENDCG
		}
	}
}