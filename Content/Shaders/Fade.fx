// Fade.fx

sampler2D TextureSampler : register(s0);

float4 FadePS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    return float4(1.0, 1.0, 1.0, texCoord.y);
}

technique Fade
{
    pass P0
    {
        PixelShader = compile ps_2_0 FadePS();
    }
}