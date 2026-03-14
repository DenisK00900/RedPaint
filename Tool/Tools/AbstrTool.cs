using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public abstract class AbstrTool
    {
        public Maincode mc;

        public string name = "None";
        public Texture2D icon;

        public string dest = "Описание инструмента";

        public AbstrTool (Maincode imc)
        {
            mc = imc;
        }

        public Vector2 GetTexPos()
        {
            return mc._image.GetTexPos();
        }

        public virtual void Update(float deltaTime)
        {

        }
    }
}
