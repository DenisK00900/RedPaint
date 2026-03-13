using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class RectSelect : AbstrTool
    {
        public RectSelect(Maincode imc) : base(imc)
        {
            name = "Выделение прямоугольником";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconRectSelect");

            dest = "Выделить указаную область\n прямоугольником";
        }

        public override void Execute()
        {

        }

    }
}
