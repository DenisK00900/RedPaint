using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public interface IDrawable
    {
        public Maincode mc { get; set; }

        public VisualElement[] visual { get; set; }

        public int depth { get; set; }

        public Vector2 GetPos();

        public void Draw(SpriteBatch sb);

        public VisualElement[] CloneVisual()
        {
            VisualElement[] clone = new VisualElement[visual.Length];

            for (int i = 0; i < clone.Length; i++)
            {
                clone[i] = visual[i];
            }

            return clone;
        }
    }
}
