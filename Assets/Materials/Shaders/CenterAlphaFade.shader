Shader "UI/CenterAlphaFade"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Width ("Fade Width", Range(0.0, 1.0)) = 1.0
        _Power ("Falloff Power", Range(0.1, 8.0)) = 1.0
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Width;
            float _Power;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;

                // distance from center in X (0 at center, 0.5 at edges)
                float d = abs(i.uv.x - 0.5);

                // map to 0..1 where 1 is center, 0 is edges
                float a = saturate(1.0 - (d / 0.5));

                // allow narrowing the band
                a = saturate(a / max(_Width, 1e-4));

                // soften / sharpen
                a = pow(a, _Power);

                c.a *= a;
                return c;
            }
            ENDCG
        }
    }
}
