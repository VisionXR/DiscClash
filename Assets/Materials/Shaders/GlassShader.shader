Shader "UI/GlassPanelURP"
{
   Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,0.12)
        _NoiseTex ("Noise", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0,0.2)) = 0.05
        _Distort ("Distortion", Range(0,0.02)) = 0.006
        _Edge ("Edge Highlight", Range(0,1)) = 0.25
        _EdgeWidth ("Edge Width", Range(0.001,0.2)) = 0.04
    }


    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_UIBlurTex);
            SAMPLER(sampler_UIBlurTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _Tint;
            float _NoiseStrength;
            float _Distort;
            float _Edge;
            float _EdgeWidth;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Noise-based distortion
                float2 noiseUV = uv * 6.0 + _Time.y * 0.15;
                float n = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r * 2 - 1;

                float2 screenUV = (IN.screenPos.xy / IN.screenPos.w);
                screenUV += n * _Distort;

                half4 bg = SAMPLE_TEXTURE2D(_UIBlurTex, sampler_UIBlurTex, screenUV);

                // Edge highlight (simple soft border)
                float2 d = abs(uv - 0.5) * 2.0;
                float edgeMask = smoothstep(1.0 - _EdgeWidth, 1.0, max(d.x, d.y));
                half3 edgeCol = half3(1,1,1) * (_Edge * edgeMask);

                // Final glass
                half3 col = bg.rgb * (1 - _Tint.a) + _Tint.rgb * _Tint.a;
                col += edgeCol;

                // Micro noise for frosted feel
                col += n * _NoiseStrength;

                half4 ui = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return half4(col, ui.a * saturate(_Tint.a + 0.08));

            }
            ENDHLSL
        }
    }
}
