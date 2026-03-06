using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public abstract class AbstrShaderTex : IDisposable
    {
        public Texture2D Tex;
        public Effect Effect;
        public RenderTarget2D Render;

        public Maincode mc;

        public abstract void Generate();

        public AbstrShaderTex(Maincode imc)
        {
            mc = imc;
        }

        public void Dispose()
        {
            Render?.Dispose();
        }
    }
}
