using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public interface IMenuElement
    {
        public bool saveParent { get; set; }

        public Vector2 GetSize();
    }
}
