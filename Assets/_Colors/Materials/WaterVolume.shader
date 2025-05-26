Shader "Unlit/WaterVolumeWorldTilingTriplanarJelly"
{
    Properties
    {
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
        _FoamMinDistance("Foam Minimum Distance", Float) = 0.04
        _TilingScale("World Tiling Scale", Float) = 1.0
        _BlendSharpness("Triplanar Blend Sharpness", Float) = 1.0 // Controls how sharp the blend is
        _JellyStrength("Jelly Strength", Float) = 0.05
        _JellySpeed("Jelly Speed", Float) = 1.0
        _JellyFrequency("Jelly Frequency", Float) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent+1"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off // Make the shader double-sided

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : NORMAL; // Pass world normal
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _TilingScale;
            float _BlendSharpness; // Declare blend sharpness

            float _JellyStrength;
            float _JellySpeed;
            float _JellyFrequency;

            v2f vert(appdata v)
            {
                v2f o;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Jelly-like vertex displacement
                float time = _Time.y * _JellySpeed;
                float displacementX = sin((worldPos.x + time) * _JellyFrequency);
                float displacementY = cos((worldPos.y + time * 1.1) * _JellyFrequency * 0.9);
                float displacementZ = sin((worldPos.z + time * 1.2) * _JellyFrequency * 1.1);

                float3 displacement = float3(displacementX, displacementY, displacementZ) * _JellyStrength;
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex) + float4(displacement, 0);
                o.vertex = UnityWorldToClipPos(worldVertex);

                o.worldPos = worldVertex.xyz;
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal); // Calculate world normal

                return o;
            }

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float4 _FoamColor;

            float _DepthMaxDistance;
            float _FoamMaxDistance;
            float _FoamMinDistance;
            float _SurfaceNoiseCutoff;

            float2 _SurfaceNoiseScroll;

            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            float4 frag(v2f i) : SV_Target
            {
                // --- Depth Calculations (Same as before) ---
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                float depthDifference = existingDepthLinear - i.screenPosition.w;
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                // --- Foam Calculations (Using worldNormal now) ---
                float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
                float3 normalDot = saturate(dot(existingNormal, i.worldNormal)); // Use worldNormal
                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDifference01 = saturate(depthDifference / foamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

                // --- Triplanar Mapping ---
                float3 blendWeights = pow(abs(i.worldNormal), _BlendSharpness);     // Calculate blend weights based on normal
                blendWeights /= (blendWeights.x + blendWeights.y + blendWeights.z); // Normalize weights

                float2 timeScroll = _Time.y * _SurfaceNoiseScroll;

                // Calculate UVs for each plane (X, Y, Z) and apply scroll
                float2 uv_xy = i.worldPos.xy * _TilingScale + timeScroll;
                float2 uv_yz = i.worldPos.yz * _TilingScale + timeScroll;
                float2 uv_xz = i.worldPos.xz * _TilingScale + timeScroll;

                // Sample texture for each plane
                float sample_xy = tex2D(_SurfaceNoise, uv_xy).r;
                float sample_yz = tex2D(_SurfaceNoise, uv_yz).r;
                float sample_xz = tex2D(_SurfaceNoise, uv_xz).r;

                // Blend the samples based on weights
                // Project XY onto Z-facing, YZ onto X-facing, XZ onto Y-facing
                float surfaceNoiseSample = sample_xy * blendWeights.z +
                    sample_yz * blendWeights.x +
                    sample_xz * blendWeights.y;

                // --- Surface Noise Application (Same as before, but using triplanar sample) ---
                float surfaceNoise = smoothstep(surfaceNoiseCutoff - 0.01, surfaceNoiseCutoff + 0.01, surfaceNoiseSample);
                float4 surfaceNoiseColor = _FoamColor;
                surfaceNoiseColor.a *= surfaceNoise;

                // --- Final Blend ---
                return lerp(waterColor, surfaceNoiseColor, surfaceNoiseColor.a);
            }
            ENDCG
        }
    }
}