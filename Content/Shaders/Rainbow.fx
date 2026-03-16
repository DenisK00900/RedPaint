// Rainbow.fx

sampler2D TextureSampler : register(s0);

float StartHue = 0.0; // Начальный оттенок (0.0 = красный, 0.33 = зелёный, 0.66 = синий)
float EndHue = 1.0; // Конечный оттенок (1.0 = снова красный, полный цикл)
float Saturation = 1.0; // Насыщенность цветов (0.0 = оттенки серого, 1.0 = яркие цвета)
float Brightness = 1.0; // Яркость (0.0 = чёрный, 1.0 = полная яркость)
float InvertY = 0.0; // Если 1.0 — инвертировать направление градиента

float3 HSVtoRGB(float3 hsv)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
    return hsv.z * lerp(K.xxx, saturate(p - K.xxx), hsv.y);
}

float4 RainbowVerticalPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float hue = lerp(StartHue, EndHue, texCoord.y);
    
    if (InvertY > 0.5)
    {
        hue = lerp(StartHue, EndHue, 1.0 - texCoord.y);
    }
    
    float3 hsv = float3(hue, Saturation, Brightness);
    float3 rgb = HSVtoRGB(hsv);
    
    return float4(rgb, 1.0);
}

technique Rainbow
{
    pass P0
    {
        PixelShader = compile ps_2_0 RainbowVerticalPS();
    }
}