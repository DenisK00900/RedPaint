// RectSelect.fx

float2 resolution;
float thicknessPixels;
bool useSmoothing;

int currCycle;
int CycleSize;

float3 color1;
float3 color2;

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
    
    float cycleFactor = (float) currCycle / (float) max(CycleSize, 1);
    
    float normX = pixelPos.x / resolution.x;
    float normY = pixelPos.y / resolution.y;
    float positionPhase = (normX + normY) * 0.5;
    
    float animatedFactor = frac(cycleFactor + positionPhase * 0.3);
    
    float t = 0.5 - 0.5 * cos(animatedFactor * 6.2831853);
    
    float3 finalColor = lerp(color1, color2, t);
    
    return float4(finalColor, alpha);
}

technique InnerEdgeOutline
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}