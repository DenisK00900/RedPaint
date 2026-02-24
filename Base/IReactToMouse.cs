using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public interface IReactToMouse
    {
        public Hitbox[] hb { get; set; }
        
        public bool mouseOver { get; set; }
    }
}
