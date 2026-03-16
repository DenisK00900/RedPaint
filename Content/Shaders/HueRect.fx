// HueRect.fx

sampler2D TextureSampler : register(s0);

float4 Color;

float4 HueRectPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float3 white = float3(1, 1, 1);
    float3 saturatedColor = lerp(white, Color.rgb, texCoord.x);
    
    float brightness = 1.0 - texCoord.y;
    float3 finalColor = saturatedColor * brightness;
    
    return float4(finalColor, Color.a);
}

technique HueRect
{
    pass P0
    {
        PixelShader = compile ps_2_0 HueRectPS();
    }
}