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
using System.Diagnostics;

namespace RedPaint
{
    public abstract class ToolSet : AbstrEntity
    {
        public string name = "Настройка";

        public abstract T GetValue<T>();

        public abstract Vector2 DetermentSize();

        public virtual Vector2 DetermentOffset()
        {
            return DetermentSize();
        }

        public virtual void DetermentPos(Vector2 newpos)
        {
            SetPos(newpos);
        }

        public virtual void UpdateHitbox()
        {

        }

        public ToolSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
