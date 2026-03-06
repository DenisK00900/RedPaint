using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RedPaint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class UpscaleTex : AbstrShaderTex
    {
        public Texture2D SourceTexture;
        public float scale;

        public override void Generate()
        {
            if (SourceTexture == null) return;

            Generate(SourceTexture, scale);
        }

        public void Generate(Texture2D tex, float sc)
        {
            Dispose();

            int newWidth = (int)(tex.Width * scale);
            int newHeight = (int)(tex.Height * scale);

            var device = mc.GraphicsDevice;

            Render = new RenderTarget2D(device, newWidth, newHeight, false,
                SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            Tex = Render;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            device.SetRenderTarget(Render);
            device.Clear(Color.Transparent);

            using (var sb = new SpriteBatch(device))
            {
                sb.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.NonPremultiplied,
                    SamplerState.PointClamp,
                    null,
                    null
                    );
                sb.Draw(tex, new Rectangle(0, 0, newWidth, newHeight), Color.White);
                sb.End();
            }

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;
        }

        public UpscaleTex(Maincode imc) : base(imc)
        {
        }
    }
}
