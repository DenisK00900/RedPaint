using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace RedPaint
{
    public class FadeTex : AbstrShaderTex
    {
        private static Texture2D _whitePixel;

        public int sizeX = 256;
        public int sizeY = 256;
        public override void Generate()
        {
            Generate(sizeX, sizeY);
        }

        public void Generate(int width, int height)
        {
            Dispose();

            var device = mc.GraphicsDevice;

            if (_whitePixel == null)
            {
                _whitePixel = new Texture2D(device, 1, 1);
                _whitePixel.SetData(new[] { Color.White });
            }

            if (Effect == null)
            {
                Effect = mc.Content.Load<Effect>("Shaders/Fade");
            }

            Render = new RenderTarget2D(device, width, height, false,
                    SurfaceFormat.Color, DepthFormat.None);

            Tex = Render;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            using (var tempSB = new SpriteBatch(device))
            {
                tempSB.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.NonPremultiplied,
                    SamplerState.LinearClamp,
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

        public FadeTex(Maincode imc) : base(imc)
        {

        }
    }
}