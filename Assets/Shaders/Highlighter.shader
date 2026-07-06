Shader "UI/Custom/AnimatedVerticalStrip"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _HighlightColor("Highlight Color", Color) = (1, 1, 0, 1)
        _StripWidth("Strip Width", Range(0.01, 0.5)) = 0.1
        _Speed("Speed", Range(0.0, 5.0)) = 1.0
        
        // Required for UI Masking/Stencil
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _HighlightColor;
            float _StripWidth;
            float _Speed;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color; 
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 spriteColor = tex2D(_MainTex, i.uv);
                
                // 1. Calculate moving position
                // We go from 0 to (1 + Width) so the strip can fully exit the right side
                float totalRange = 1.0 + _StripWidth;
                float currentOffset = fmod(_Time.y * _Speed, totalRange);
                
                // 2. Define the moving boundaries
                float leftBoundary = currentOffset - _StripWidth;
                float rightBoundary = currentOffset;

                fixed4 finalCol;

                // 3. Logic: If UV.x is inside the moving strip
                if (i.uv.x > leftBoundary && i.uv.x < rightBoundary) 
                {
                    finalCol = _HighlightColor;
                }
                else 
                {
                    finalCol = spriteColor;
                }

                // 4. Apply alpha transparency from the original sprite and UI tint
                finalCol.a *= spriteColor.a * i.color.a;
                
                return finalCol;
            }
            ENDCG
        }
    }
}