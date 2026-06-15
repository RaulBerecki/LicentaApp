// Runtime hologram/ghost shader for URP. Used as the placement preview for models
// downloaded at runtime (which have no dedicated "_placing" preview prefab).
// Additive blend + fresnel rim glow + horizontal scanlines = classic hologram look.
Shader "Custom/HologramURP"
{
    Properties
    {
        _Color ("Tint", Color) = (0.3, 0.7, 1, 1)
        _RimColor ("Rim Color", Color) = (0.5, 0.9, 1, 1)
        _RimPower ("Rim Power", Range(0.5, 8)) = 3
        _Alpha ("Base Alpha", Range(0, 1)) = 0.2
        _ScanSpeed ("Scanline Speed", Float) = 2
        _ScanDensity ("Scanline Density", Float) = 25
        _ScanStrength ("Scanline Strength", Range(0, 1)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha One   // additive glow
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _RimColor;
                float _RimPower;
                float _Alpha;
                float _ScanSpeed;
                float _ScanDensity;
                float _ScanStrength;
            CBUFFER_END

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float3 N = normalize(IN.normalWS);
                float3 V = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float fresnel = pow(1.0 - saturate(dot(N, V)), _RimPower);

                // Horizontal scanlines that scroll over time.
                float scan = sin(IN.positionWS.y * _ScanDensity + _Time.y * _ScanSpeed);
                scan = saturate(scan) * _ScanStrength;

                float3 col = _Color.rgb + _RimColor.rgb * fresnel + scan.xxx;
                float alpha = saturate(_Alpha + fresnel);
                return half4(col, alpha);
            }
            ENDHLSL
        }
    }
    Fallback Off
}
