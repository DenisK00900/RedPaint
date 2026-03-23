using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class CircleDraw : AbstrTool
    {
        public BlockRender blockRender;

        public CircleDraw(Maincode imc) : base(imc)
        {
            name = "Круг";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconCircle");

            dest = "Рисование круга";

            blockRender = new BlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            blockRender.size = (int)(scale);

            blockRender.thickness = 2f;

            blockRender.Generate();

            return blockRender.Tex;
        }
    }
}
