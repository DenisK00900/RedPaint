using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RedPaint
{
    public class RectDraw : AbstrTool
    {
        public BlockRender blockRender;

        public RectDraw(Maincode imc) : base(imc)
        {
            name = "Прямоугольник";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconRect");

            dest = "Рисование прямоугольника";

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
