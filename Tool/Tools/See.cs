using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class See : AbstrTool
    {
        public See(Maincode imc) : base(imc)
        {
            name = "Просмотр";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconSee");

            dest = "Просмотр изображения\nбез изменений";
        }

        public override void Execute()
        {

        }

    }
}
