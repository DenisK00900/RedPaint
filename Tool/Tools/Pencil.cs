using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class Pencil : AbstrTool
    {
        public Pencil(Maincode imc) : base(imc)
        {
            name = "Карандаш";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconPencil");

            dest = "Простой инструмент, который\nкрасит пиксели в определённый цвет";
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
