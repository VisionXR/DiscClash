Shader "UI/Custom/CircleGenerator"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Radius("Radius", Range(0.0, 0.5)) = 0.5
        _Softness("Edge Softness", Range(0.0, 0.1)) = 0.01
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
            float _Radius;
            float _Softness;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // 1. Calculate distance from center (0.5, 0.5)
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // 2. Determine Alpha
                // Simple version: if (dist < _Radius) alpha = 1 else 0
                // Smooth version: use smoothstep for anti-aliasing (no jagged edges)
                float alpha = smoothstep(_Radius, _Radius - _Softness, dist);

                // 3. Construct color (using i.color for UI Tint support)
                fixed4 col = fixed4(i.color.rgb, i.color.a * alpha);
                
                return col;
            }
            ENDCG
        }
    }
}