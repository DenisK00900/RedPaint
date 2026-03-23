using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class See : AbstrTool
    {
        public BlockRender blockRender;

        public See(Maincode imc) : base(imc)
        {
            name = "Просмотр";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconSee");

            dest = "Просмотр изображения\nбез изменений";

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
