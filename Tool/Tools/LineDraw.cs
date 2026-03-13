using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class LineDraw : AbstrTool
    {
        public LineDraw(Maincode imc) : base(imc)
        {
            name = "Линия";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconLine");

            dest = "Рисование прямой линии";
        }

        public override void Execute()
        {

        }

    }
}
