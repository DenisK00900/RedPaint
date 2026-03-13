using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class Fill : AbstrTool
    {
        public Fill(Maincode imc) : base(imc)
        {
            name = "Заливка";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconFill");

            dest = "Закрашивает область в\nопределённый цвет";
        }

        public override void Execute()
        {

        }

    }
}
