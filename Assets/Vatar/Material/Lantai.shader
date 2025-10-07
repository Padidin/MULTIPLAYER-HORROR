Shader "Custom/WorldTiled"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tiling ("Tiling", Vector) = (1,1,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        sampler2D _MainTex;
        float4 _Tiling;

        struct Input {
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.worldPos.xz * _Tiling.xy;
            o.Albedo = tex2D(_MainTex, uv).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
