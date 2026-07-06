Shader "UI/Custom/FadeHorizontal"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        _cutOff("CutOff Length", Range(0.001, 0.5)) = 0.1
         _topCutOff("top cutoff", Range(0.001, 0.1)) = 0.01
        _centerAlpha("Center Alpha (0-255)", Range(0, 255)) = 150
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
        }
        
        // CRITICAL: UI shaders need Blending enabled to see transparency
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
                fixed4 color : COLOR; // Pass vertex color (Tint) to fragment
            };

            // You must declare these variables to match Properties
            sampler2D _MainTex;
            fixed4 _Color;
            float _cutOff;
            float _centerAlpha;
            float _topCutOff;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color; // Combine Sprite Renderer tint + Material color
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float finalAlpha = 0;
                float targetAlpha = _centerAlpha / 255.0;
                fixed4 col = fixed4(0, 0, 0, 0);

                // Improved logic flow
                if (uv.x < _cutOff) {
                    // Fade In: Progresses from 0 to targetAlpha
                    finalAlpha = (uv.x / _cutOff) * targetAlpha;
                }
                else if (uv.x > (1.0 - _cutOff)) {
                    // Fade Out: Progresses from targetAlpha to 0
                    finalAlpha = ((1.0 - uv.x) / _cutOff) * targetAlpha;
                   
                }
                else {
                    // Middle section
                    finalAlpha = targetAlpha;
                    
                }
                col = fixed4(0, 0, 0, finalAlpha);

                 if (uv.y < _topCutOff) {

                    
                          col = fixed4(1, 1, 1, finalAlpha);
                      
                 }
                 else  if (uv.y > 1-_topCutOff)
                 {
                   
                         col = fixed4(1, 1, 1, finalAlpha);
                      
                }

               
                return col * i.color;
            }
            ENDCG
        }
    }
}