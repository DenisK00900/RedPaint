using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class CircleDraw : AbstrTool
    {
        public CircleDraw(Maincode imc) : base(imc)
        {
            name = "Круг";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconCircle");

            dest = "Рисование круга";
        }
    }
}
