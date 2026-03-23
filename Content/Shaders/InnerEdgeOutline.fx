// InnerEdgeOutline.fx

float2 resolution;
float thicknessPixels;
bool useSmoothing;

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    float2 pixelPos = uv * resolution;

    float distToLeft = pixelPos.x;
    float distToRight = resolution.x - pixelPos.x;
    float distToTop = pixelPos.y;
    float distToBottom = resolution.y - pixelPos.y;
    
    float minDistToEdge = min(min(distToLeft, distToRight),
                              min(distToTop, distToBottom));
    
    float alpha;
    
    if (thicknessPixels <= 0.0)
    {
        alpha = 0.0;
    }
    else
    {
        if (useSmoothing)
        {
            alpha = 1.0 - smoothstep(0.0, thicknessPixels, minDistToEdge);
        }
        else
        {
            alpha = 1.0 - step(thicknessPixels, minDistToEdge);
        }
    }
    
    return float4(1.0, 1.0, 1.0, alpha);
}

technique InnerEdgeOutline
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}