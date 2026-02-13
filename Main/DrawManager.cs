using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedPaint
{
    public class DrawManager
    {
        public Maincode mc;

        public List<IDrawable> drawables;

        public DrawManager(Maincode parent)
        {
            mc = parent;
        }

        public void Reorganaze()
        {
            drawables.Sort((x, y) => x.depth.CompareTo(y.depth));
        } 

        public List<IDrawable> GetDrawables()
        {
            List<IDrawable> di = new List<IDrawable>();

            foreach (AbstrEntity item in mc.entities)
            {
                if (item is IDrawable d)
                {
                    di.Add(d);
                }
            }

            return di;
        }

        public void Draw(SpriteBatch sb)
        {
            drawables = GetDrawables();

            Reorganaze();

            foreach (IDrawable drawable in drawables)
            {
                drawable.Draw(sb);
            }
        }
    }
}