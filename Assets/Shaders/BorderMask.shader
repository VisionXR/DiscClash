Shader "UI/BorderMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color   ("Tint", Color) = (1,1,1,1)
        _BorderPixels ("Border (px)", Float) = 10
        _FeatherPixels ("Feather (px)", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            // texel size (x = 1/width, y = 1/height)
            float4 _MainTex_TexelSize;
            float4 _Color;
            float _BorderPixels;
            float _FeatherPixels;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 col : COLOR;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.col = v.color * _Color;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                // _MainTex_TexelSize is provided by Unity: (1/width, 1/height, width, height)
                float2 texelSize = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);

                // convert border pixels to UV-space
                float2 borderUV = _BorderPixels * texelSize;

                // compute inner rect bounds
                float2 innerMin = float2(borderUV.x, borderUV.y);
                float2 innerMax = float2(1.0 - borderUV.x, 1.0 - borderUV.y);

                // determine if pixel lies in the border region
                bool inBorder = (uv.x < innerMin.x) || (uv.x > innerMax.x) || (uv.y < innerMin.y) || (uv.y > innerMax.y);

                // optional inner edge feathering
                float alphaMask = inBorder ? 1.0 : 0.0;
                if (_FeatherPixels > 0.0)
                {
                    float2 featherUV = _FeatherPixels * texelSize;
                    // distance to inner rectangle edge (positive inside the inner rect)
                    float dx = min(max(uv.x - innerMin.x, 0.0), max(innerMax.x - uv.x, 0.0));
                    float dy = min(max(uv.y - innerMin.y, 0.0), max(innerMax.y - uv.y, 0.0));
                    float insideDist = max(dx, dy); // 0 at inner edge, positive inside
                    // when insideDist < feather, alpha ramps from 1 -> 0
                    float feather = max(featherUV.x, featherUV.y);
                    if (insideDist < feather)
                    {
                        alphaMask = saturate(insideDist / feather); // 0..1
                        alphaMask = 1.0 - alphaMask; // invert so near edge = 1, center = 0
                    }
                    else
                    {
                        alphaMask = 0.0;
                    }
                }

                float4 tex = tex2D(_MainTex, uv);
                float4 outCol = tex * i.col;
                outCol.a *= alphaMask;

                // discard completely transparent pixels optionally (helps some batching)
                clip(outCol.a - 1e-5);

                return outCol;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}