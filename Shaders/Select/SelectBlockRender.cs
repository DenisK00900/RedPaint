using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace RedPaint
{
    public class SelectBlockRender : AbstrShaderTex
    {
        private static Texture2D _whitePixel;

        public int sizeX = 256;
        public int sizeY = 256;

        public float thickness = 4f;
        public bool smoth = false;

        public Color color1 = Color.White;
        public Color color2 = Color.Cyan;

        public int cycleSize = 60;
        public int currCycle = 0;

        public bool enableAnimation = true;
        public float phaseShiftFactor = 0.3f;

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
                Effect = mc.Content.Load<Effect>("Shaders/RectSelect");
            }

            int rsizeX = Math.Max(1, sizeX);
            int rsizeY = Math.Max(1, sizeY);

            Render = new RenderTarget2D(device, rsizeX, rsizeY, false,
                    SurfaceFormat.Color, DepthFormat.None);

            Tex = Render;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            Effect.Parameters["resolution"].SetValue(new Vector2(rsizeX, rsizeY));
            Effect.Parameters["thicknessPixels"].SetValue(thickness);
            Effect.Parameters["useSmoothing"].SetValue(smoth);

            Effect.Parameters["color1"].SetValue(color1.ToVector3());
            Effect.Parameters["color2"].SetValue(color2.ToVector3());

            Effect.Parameters["CycleSize"].SetValue(enableAnimation ? cycleSize : 1);
            Effect.Parameters["currCycle"].SetValue(enableAnimation ? currCycle : 0);

            // Effect.Parameters["phaseShiftFactor"].SetValue(phaseShiftFactor);

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

                tempSB.Draw(_whitePixel, new Rectangle(0, 0, rsizeX, rsizeY), Color.White);

                tempSB.End();
            }

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;
        }

        public void AdvanceCycle(int steps = 1)
        {
            if (cycleSize <= 0) cycleSize = 1;
            currCycle = (currCycle + steps) % cycleSize;
        }

        public SelectBlockRender(Maincode imc) : base(imc)
        {
        }
    }
}