// CircleUtility.fx

float2 resolution;
float radiusPixels;
float thicknessPixels;
bool useSmoothing;

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    float2 pixelPos = uv * resolution;
    float2 centerPixel = resolution * 0.5;
    float distanceFromCenter = length(pixelPos - centerPixel);

    float distanceFromIdealRadius = abs(distanceFromCenter - radiusPixels);

    float alpha;
    float halfThickness = thicknessPixels * 0.5;

    if (halfThickness <= 0)
    {
        alpha = 0.0;
    }
    else
    {
        if (useSmoothing)
        {
            alpha = 1.0 - smoothstep(0.0, halfThickness, distanceFromIdealRadius);
        }
        else
        {
            alpha = 1.0 - step(halfThickness, distanceFromIdealRadius);
        }
    }

    return float4(1, 1, 1, alpha);
}

technique
{
    pass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}