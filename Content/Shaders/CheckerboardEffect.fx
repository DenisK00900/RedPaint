sampler2D TextureSampler : register(s0);

float4 WhiteOutPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(TextureSampler, texCoord);
    
    if (texColor.a > 0)
    {
        return float4(1, 1, 1, texColor.a);
    }
    else
    {
        return texColor;
    }
}

technique WhiteOut
{
    pass P0
    {
        PixelShader = compile ps_2_0 WhiteOutPS();
    }
}