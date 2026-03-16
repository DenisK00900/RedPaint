using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RedPaint
{
    public class RainbowTex : AbstrShaderTex
    {
        private static Texture2D _whitePixel;

        public int sizeX = 64;
        public int sizeY = 256;

        public float startHue = 0.0f;
        public float endHue = 1.0f;
        public float saturation = 1.0f;
        public float brightness = 1.0f;
        public bool invertY = false;

        public override void Generate()
        {
            Generate(sizeX, sizeY, startHue, endHue, saturation, brightness, invertY);
        }

        public void Generate(int width, int height, float startHue, float endHue,
            float saturation, float brightness, bool invertY)
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
                Effect = mc.Content.Load<Effect>("Shaders/Rainbow");
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

            Effect.Parameters["StartHue"].SetValue(startHue);
            Effect.Parameters["EndHue"].SetValue(endHue);
            Effect.Parameters["Saturation"].SetValue(saturation);
            Effect.Parameters["Brightness"].SetValue(brightness);
            Effect.Parameters["InvertY"].SetValue(invertY ? 1.0f : 0.0f);

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

        public void Generate(int width, int height)
        {
            Generate(width, height, startHue, endHue, saturation, brightness, invertY);
        }

        public RainbowTex(Maincode imc) : base(imc)
        {
        }
    }
}