using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace RedPaint
{
    public class LineDraw : AbstrTool
    {
        public BlockRender blockRender;

        public LineDraw(Maincode imc) : base(imc)
        {
            name = "Линия";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconLine");

            dest = "Рисование прямой линии";

            blockRender = new BlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            blockRender.sizeX = (int)(scale);
            blockRender.sizeY = (int)(scale);

            blockRender.thickness = 2f;

            blockRender.Generate();

            return blockRender.Tex;
        }
    }
}
