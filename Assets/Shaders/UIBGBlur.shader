Shader "UI/UIBGBlur"
{
    Properties
    {
        _Size ("Blur Size", Range(0, 20)) = 5.0
        _Color ("Tint Color", Color) = (1,1,1,1)
        
        // Required for Unity UI Components
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline" // Explicitly marks this for URP
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "UI_Blur"
            HLSLPROGRAM // Swapped CGPROGRAM to modern HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Core URP shader libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float4 uv       : TEXCOORD0; // Screen-space UV coordinates
                float4 color    : COLOR;
            };

            float _Size;
            float4 _Color;

            // This tells the compiler to look up URP's built-in background pass texture safely
            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);
            float4 _CameraOpaqueTexture_TexelSize;

            v2f vert(appdata_t v)
            {
                v2f o;
                // Convert object vertex position to screen clip space via modern macro
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.vertex = vertexInput.positionCS;
                
                // Generates seamless, stretch-proof coordinate mapping for screen space
                float4 screenPos = ComputeScreenPos(o.vertex);
                o.uv = screenPos;
                
                o.color = v.color * _Color;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Unpack projection coordinates to standard 2D UV mapping coordinates
                float2 uv = i.uv.xy / i.uv.w;
                float2 texelSize = _CameraOpaqueTexture_TexelSize.xy * _Size;

                // Simple box-blur sampling 5 distinct points
                float4 sum = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv);
                sum += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(texelSize.x, texelSize.y));
                sum += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(-texelSize.x, -texelSize.y));
                sum += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(texelSize.x, -texelSize.y));
                sum += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(-texelSize.x, texelSize.y));
                
                float4 finalBlur = (sum / 5.0) * i.color;
                return finalBlur;
            }
            ENDHLSL
        }
    }
}