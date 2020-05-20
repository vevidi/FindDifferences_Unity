Shader "Custom/CopyArea"
{
    Properties
    {
        _MainTex ("Texture", any) = "" {}
        _AreaParams ("Scale and Offset", Vector) = (1.0, 1.0, 0.0, 0.0)
    }
 
    SubShader
    {
        Pass
        {
             ZTest Always
             Cull Off
             ZWrite Off
            CGPROGRAM
     
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc" // todo OLD
            sampler2D _MainTex;
            float4 _AreaParams;
            uniform float4 _MainTex_ST; // todo OLD
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
         
                //o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex); // todo OLD
                o.texcoord = v.texcoord.xy * _AreaParams.xy + _AreaParams.zw; // todo NEW
         
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.texcoord);
            }
     
            ENDCG
        }
    }
    Fallback Off
}