Shader "Custom/RevolvingTorusGlow"
{
    Properties
    {
        _GlowColor("Tint", Color) = (1,1,1,1)
        _Speed("Revolve Speed", Float) = 1.0
        _HueSpread("Hue Spread (0..1..2)", Range(0,2)) = 1.0
        _RimPower("Rim Power", Range(0.5,8)) = 2.0
        _Emission("Emission Mult", Range(0,10)) = 2.0
        _Pulse("Pulse (0..1)", Range(0,1)) = 0.0 // set by script
    }
    SubShader
    {
        Tags{ "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One        // additive glow
        ZWrite Off
        Cull Back
        Lighting Off
        Fog{ Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _GlowColor;
            float  _Speed, _HueSpread, _RimPower, _Emission, _Pulse;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f {
                float4 pos    : SV_POSITION;
                float3 osPos  : TEXCOORD0;
                float3 wNrm   : TEXCOORD1;
                float3 wView  : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.osPos = v.vertex.xyz;

                float3 wPos  = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 wNrm  = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float3 wView = normalize(_WorldSpaceCameraPos - wPos);

                o.wNrm  = wNrm;
                o.wView = wView;
                return o;
            }

            // Simple HSV->RGB
            float3 hsv2rgb(float3 c){
                float4 K = float4(1., 2./3., 1./3., 3.);
                float3 p = abs(frac(c.xxx + K.xyz) * 6. - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Revolve color around Y by object-space angle in XZ
                float2 xz   = normalize(i.osPos.xz + 1e-5);
                float angle = atan2(xz.y, xz.x);             // -PI..PI
                float hue   = frac(angle / (2*UNITY_PI) + _Time.y * _Speed);
                float3 col  = hsv2rgb(float3(hue * _HueSpread, 1, 1));

                // Rim to enhance edge glow
                float rim   = pow(1 - saturate(dot(i.wNrm, i.wView)), _RimPower);

                // Pulse controls visibility (script animates 0..1)
                float intensity = _Emission * rim * saturate(_Pulse);

                float3 rgb = col * intensity * _GlowColor.rgb;
                return float4(rgb, 1);
            }
            ENDCG
        }
    }
    FallBack Off
}
