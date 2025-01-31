Shader "Post/CRT" {
    Properties {
        _CRTTex ("CRT Texture", 2D) = "black" {}
        _NoBandTex ("No Banding Texture", 2D) = "black" {}
        _Curvature ("Curvature", float) = 1.0
        _Power ("Power", Range(2, 100)) = 2.0
        _Depth ("Depth", float) = 1.0
        _BandingFreq ("Banding Frequency", float) = 512
        _Opacity ("Opacity", Range(0, 2)) = 1.0
        _Brightness ("Brightness", Range(0, 2)) = 1.0
        _VignetteRoundness ("Vignette Roundness", Range(0.01, 0.5)) = 0.5
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            uniform Texture2D _CRTTex;
            uniform Texture2D _NoBandTex;
            uniform float _Curvature;
            uniform float _Depth;
            uniform float _BandingFreq;
            uniform float _Opacity;
            uniform float _Brightness;
            uniform float _Power;
            uniform float _VignetteRoundness;

            uniform SamplerState linear_clamp_sampler;

            // Bend the screen
            float2 get_new_uv(float2 uv)
            {
                float2 norm_uv = uv - float2(0.5, 0.5);
                float depth = _Curvature * dot(float2(1, 1), pow(abs(norm_uv), _Power));
                return clamp(norm_uv * (_Depth + depth) + float2(0.5, 0.5), float2(0, 0), float2(1, 1));
            }

            //Generate sin transformed to bounds low and high to create pixels
            float2 get_sin_bounds(float2 uv, float2 low, float2 high)
            {
                float2 sin_uv = sin(uv * TAU);
                return (sin_uv + float2(1, 1)) * (high - low) / 2 + low;
            }

            float4 frag(v2f_img i) : COLOR {
                float2 uv = get_new_uv(i.uv);
                float4 raw_col = _CRTTex.Sample(linear_clamp_sampler, uv);
                float2 sins = get_sin_bounds(uv * _BandingFreq, float2(0.1, 0.1), float2(1.0, 1.0));

                float3 unbanded_col = min(raw_col.xyz * _Brightness.xxx, 1);
                float4 banded_col = float4(unbanded_col * pow(sins.x * sins.y, _Opacity), 1.0);
                float4 no_band_col = _NoBandTex.Sample(linear_clamp_sampler, uv);
                float4 col = no_band_col.a < 0.5 ? banded_col : no_band_col;
                
                float2 vignette = -pow(2.0 * abs(uv - float2(0.5, 0.5)), 1.0 / _VignetteRoundness) + float2(1, 1);
                return col * vignette.x * vignette.y;
            }
            ENDCG
        }
    }
}