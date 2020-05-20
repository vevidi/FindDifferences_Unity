// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UnlitGrayscaleMasked" 
{
	 Properties 
	 {
		 _MainTex ("Texture", 2D) = "white" { }
		 _AlphaTex ("External Alpha", 2D) = "white" {}
	 }

	 SubShader 
	 {
		Tags {  "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjectors"="True" } 

		Cull Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

		 Pass 
		{ 
			 CGPROGRAM
			 #pragma vertex vert
			 #pragma fragment frag
 
			 #include "UnityCG.cginc"
 
 			 sampler2D _MainTex;
			 sampler2D _AlphaTex;         
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

			 struct v2f {
				 float4  pos : SV_POSITION;
				 float2  uv : TEXCOORD0;
			 };
 
			 float4 _MainTex_ST;
 
			 v2f vert (appdata_t v)
			 {
				 v2f o;
				 o.pos = UnityObjectToClipPos (v.vertex);
				 o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				 return o;
			 }
 
			 half4 frag (v2f i) : SV_Target
			 {
				 half4 texcol = tex2D (_MainTex, i.uv); 
				 half4 texcol_a = tex2D (_AlphaTex, i.uv);
				 texcol.rgb = dot(texcol.rgb, float3(0.3, 0.59, 0.11));
				 return fixed4(texcol.r, texcol.g, texcol.b, texcol_a.a);
			 }
			 ENDCG 
		 }
	 }
 } 