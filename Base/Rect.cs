using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Rect
    {
        public Vector2 position;

        public Vector2 size;

        public bool CheckPoint(Vector2 point)
        {
            return 
                point.X >= position.X && 
                point.Y >= position.Y &&
                point.X <= position.X + size.X &&
                point.Y <= position.Y + size.Y;
        }

        public Rect GetSubrect(int w, int h, int x, int y)
        {
            if (w <= 0 || h <= 0) throw new ArgumentException("...");
            float subWidth = size.X / w;
            float subHeight = size.Y / h;
            return new Rect(
                new Vector2(position.X + x * subWidth, position.Y + y * subHeight),
                new Vector2(subWidth, subHeight));
        }

        public Rect()
        {

        }

        public Rect(Vector2 pos, Vector2 s)
        {
            position = pos;
            size = s;
        }

        public Rect(Vector2 pos, Vector2 s, Vector2 origin)
        {
            size = s;

            position = pos - size * origin;
        }
    }
}
