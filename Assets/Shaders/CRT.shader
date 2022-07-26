Shader "Post/CRT" {
    Properties {
        _CRTTex ("CRT Texture", 2D) = "black" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _CRTTex;

            float4 frag(v2f_img i) : COLOR {
                return tex2D(_CRTTex, i.uv);
            }
            ENDCG
        }
    }
}