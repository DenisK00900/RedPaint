using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RedPaint
{
    public class CheckerTex : AbstrShaderTex
    {
        private static Texture2D _whitePixel;

        public int sizeX = 256;
        public int sizeY = 256;

        public int sizeChecker = 32;

        public Color color1;
        public Color color2;

        public override void Generate()
        {
            Generate(sizeX, sizeY, sizeChecker, color1, color2);
        }

        public void Generate(int width, int height, int cellSize, Color color1, Color color2)
        {
            var device = mc.GraphicsDevice;

            if (_whitePixel == null)
            {
                _whitePixel = new Texture2D(device, 1, 1);
                _whitePixel.SetData(new[] { Color.White });
            }

            if (Effect == null)
            {
                Effect = mc.Content.Load<Effect>("Shaders/CheckerboardEffect");
            }

            if (Render == null || Render.Width != width || Render.Height != height)
            {
                Render = new RenderTarget2D(device, width, height, false,
                    SurfaceFormat.Color, DepthFormat.None);

                Tex = Render;
            }

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            Effect.Parameters["Color1"].SetValue(color1.ToVector4());
            Effect.Parameters["Color2"].SetValue(color2.ToVector4());
            Effect.Parameters["CellSize"].SetValue(new Vector2(cellSize, cellSize));
            Effect.Parameters["SurfaceSize"].SetValue(new Vector2(width, height));

            using (var tempSB = new SpriteBatch(device))
            {
                tempSB.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                null,
                null,
                Effect
                );

                tempSB.Draw(_whitePixel, new Rectangle(0, 0, width, height), Color.White);

                tempSB.End();
            }

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;
        }

        public CheckerTex(Maincode imc) : base(imc)
        {

        }
    }
}