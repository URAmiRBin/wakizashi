Shader "Unlit/Intersection"
{     
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _IntersectIntensity("Intersection Intensity", float) = 10.0
        _IntersectExponent("Intersection Falloff Exponent", float) = 6.0
    }
    SubShader {
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
        Cull Off
        Blend SrcAlpha One

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD2;
                float depth : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            sampler2D _CameraDepthNormalsTexture;
            float _IntersectIntensity;
            float _IntersectExponent;


            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float diff = DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy / i.screenPos.w).zw) - i.depth;
                float intersectGradient = 1 - min(diff / _ProjectionParams.w, 1.0f);
                fixed4 intersectTerm = _Color * pow(intersectGradient, _IntersectExponent) * _IntersectIntensity;
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed4 mainTerm = mainTex * _Color;
                return fixed4(_Color.rgb + mainTerm.rgb + intersectTerm, _Color.a);
            }

            ENDHLSL
        }
    } 
}