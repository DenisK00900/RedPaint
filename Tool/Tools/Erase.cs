using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class Erase : AbstrTool
    {
        public Erase(Maincode imc) : base(imc)
        {
            name = "Ластик";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconErase");

            dest = "Стирает цвет пикселя,\nвозвращая его к прозрачности\nили цвету фона";
        }
    }
}
