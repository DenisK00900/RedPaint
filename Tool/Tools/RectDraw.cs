using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class RectDraw : AbstrTool
    {
        public RectDraw(Maincode imc) : base(imc)
        {
            name = "Прямоугольник";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconRect");

            dest = "Рисование прямоугольника";
        }

        public override void Execute()
        {

        }

    }
}
