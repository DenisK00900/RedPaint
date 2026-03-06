//ChopEffect.fx

sampler2D TextureSampler : register(s0);

float4 CropMargins;
float2 TextureSize;

float4 CropPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 pixelPos = texCoord * TextureSize;
    
    float cropTop = CropMargins.x;
    float cropRight = CropMargins.y;
    float cropBottom = CropMargins.z;
    float cropLeft = CropMargins.w;

    bool isCropped = 
        pixelPos.x < cropLeft ||
        pixelPos.x >= TextureSize.x - cropRight ||
        pixelPos.y < cropTop ||
        pixelPos.y >= TextureSize.y - cropBottom;
    
    if (isCropped)
    {
        return float4(0, 0, 0, 0);
    }
    
    return tex2D(TextureSampler, texCoord);
}

technique CropMarginsTech
{
    pass P0
    {
        PixelShader = compile ps_2_0 CropPS();
    }
}