Shader "Custom/ProceduralVolumeNoise"
{
    Properties
    {
        _NoiseScale("Noise Scale", Float) = 1.0
        _Tiling("Tiling", Float) = 1.0
        _ScrollDirection("Noise Scroll Direction (xyz)", Vector) = (0.5, 0.5, 0.2, 0)
        _AlphaThreshold("Alpha Threshold", Range(0,1)) = 0.5
        _AlphaSmooth("Alpha Smoothness", Range(0,1)) = 0.1
        _Color("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _NoiseScale;
            float _Tiling;
            float4 _ScrollDirection;
            float _AlphaThreshold;
            float _AlphaSmooth;
            fixed4 _Color;

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            // Hash function for pseudo-random value generation
            float hash(float3 p)
            {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            // Trilinear interpolated value noise
            float valueNoise(float3 p)
            {
                float3 pi = floor(p);
                float3 pf = frac(p);
                float3 w = pf * pf * (3.0 - 2.0 * pf); // smoothstep

                float c000 = hash(pi + float3(0, 0, 0));
                float c100 = hash(pi + float3(1, 0, 0));
                float c010 = hash(pi + float3(0, 1, 0));
                float c110 = hash(pi + float3(1, 1, 0));
                float c001 = hash(pi + float3(0, 0, 1));
                float c101 = hash(pi + float3(1, 0, 1));
                float c011 = hash(pi + float3(0, 1, 1));
                float c111 = hash(pi + float3(1, 1, 1));

                float x00 = lerp(c000, c100, w.x);
                float x10 = lerp(c010, c110, w.x);
                float x01 = lerp(c001, c101, w.x);
                float x11 = lerp(c011, c111, w.x);
                float y0 = lerp(x00, x10, w.y);
                float y1 = lerp(x01, x11, w.y);
                return lerp(y0, y1, w.z);
            }

            // Fractal Brownian Motion (FBM) for smoother noise
            float fbm(float3 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;

                for(int i = 0; i < 4; i++)
                {
                    value += amplitude * valueNoise(p * frequency);
                    frequency *= 2.0;
                    amplitude *= 0.5;
                }

                return value;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 pos = i.worldPos;

                // Scroll through space with time
                float3 scroll = _Time.y * _ScrollDirection.xyz;
                float3 noiseCoord = (pos + scroll) * _Tiling * _NoiseScale;

                // Compute fractal noise
                float n = fbm(noiseCoord);

                // Smooth alpha masking
                float alpha = smoothstep(_AlphaThreshold - _AlphaSmooth, _AlphaThreshold + _AlphaSmooth, n);

                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}