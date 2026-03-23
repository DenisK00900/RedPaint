using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class BlockRender : AbstrShaderTex
    {
        private static Texture2D _whitePixel;

        public int size = 256;

        public float thickness = 4f;

        public bool smoth = false;

        public override void Generate()
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
                Effect = mc.Content.Load<Effect>("Shaders/InnerEdgeOutline");
            }

            int rsize = Math.Max(1, size);

            Render = new RenderTarget2D(device, rsize, rsize, false,
                    SurfaceFormat.Color, DepthFormat.None);

            Tex = Render;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            Effect.Parameters["resolution"].SetValue(new Vector2(rsize, rsize));
            Effect.Parameters["thicknessPixels"].SetValue(thickness);
            Effect.Parameters["useSmoothing"].SetValue(smoth);

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

                tempSB.Draw(_whitePixel, new Rectangle(0, 0, rsize, rsize), Color.White);

                tempSB.End();
            }

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;
        }

        public BlockRender(Maincode imc) : base(imc)
        {
        }
    }
}
