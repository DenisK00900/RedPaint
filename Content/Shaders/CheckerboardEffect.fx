//CheckerboardEffect

sampler2D TextureSampler : register(s0);

float4 Color1; // Первый цвет (верхний левый угол)
float4 Color2; // Второй цвет
float2 CellSize; // Размер одной клетки в пикселях (X, Y)
float2 SurfaceSize; // Размер области рисования в пикселях (Ширина, Высота)

float4 CheckeredPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 pos = texCoord * SurfaceSize;

    float2 grid = floor(pos / CellSize);
    
    float sum = grid.x + grid.y;
    
    float parity = frac(sum * 0.5);
    
    if (parity > 0.1)
    {
        return Color2;
    }
    else
    {
        return Color1;
    }
}

technique Checkered
{
    pass P0
    {
        PixelShader = compile ps_2_0 CheckeredPS();
    }
}