using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RedPaint
{
    public class ChopTex : AbstrShaderTex
    {
        public Texture2D SourceTexture;
        public Vector4 cropMargins = Vector4.Zero;

        public ChopTex(Maincode imc) : base(imc)
        {
        }

        public override void Generate()
        {
            if (SourceTexture == null) return;

            Generate(SourceTexture, cropMargins);
        }

        public void Generate(Texture2D source, Vector4 margins)
        {
            if (source == null) return;

            Dispose();

            SourceTexture = source;
            cropMargins = margins;

            var device = mc.GraphicsDevice;

            if (Effect == null)
            {
                Effect = mc.Content.Load<Effect>("Shaders/ChopEffect");
            }

            Render = new RenderTarget2D(device, source.Width, source.Height, false,
                    SurfaceFormat.Color, DepthFormat.None);

            Tex = Render;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            Effect.Parameters["CropMargins"].SetValue(cropMargins);
            Effect.Parameters["TextureSize"].SetValue(new Vector2(source.Width, source.Height));

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

                tempSB.Draw(source, new Rectangle(0, 0, source.Width, source.Height), Color.White);

                tempSB.End();
            }

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;
        }
    }
}