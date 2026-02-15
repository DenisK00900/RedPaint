using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Sprite : VisualElement
    {

        public IDrawable parent { get; set; } = null;

        public Vector2 pos { get; set; } = Vector2.Zero;

        public float rotation { get; set; } = 0;

        public Vector2 scale { get; set; } = Vector2.One;

        public bool isAbsolute { get; set; } = false;

        public Texture2D texture { get; set; } = null;

        public Vector2? origin { get; set; } = null;

        public Color color { get; set; } = Color.White;

        public float alpha { get; set; } = 1f;

        public bool isActive { get; set; } = true;

        public int index { get; set; }
       
        public void Draw(SpriteBatch sb)
        {
            if (!isActive) return;

            if (TUH.InsideScreen(pos, parent.mc._data, Math.Max(texture.Width, texture.Height)))
            {
                Vector2 lorigin = origin.HasValue ? origin.Value : new Vector2(texture.Width / 2f, texture.Height / 2f);

                sb.Draw(
                    texture,
                    parent.GetPos() + pos,
                    null,
                    color * alpha,
                    MathHelper.ToRadians(rotation),
                    lorigin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public Sprite(IDrawable pr)
        {
            parent = pr;

            Random random = new Random();

            index = random.Next(0, 9999999);
        }

        public VisualElement Clone()
        {
            Sprite clone = new Sprite(parent);

            clone.pos = pos;
            clone.rotation = rotation;
            clone.scale = scale;
            clone.isAbsolute = isAbsolute;
            clone.texture = texture;
            clone.origin = origin;
            clone.color = color;
            clone.alpha = alpha;
            clone.isActive = isActive;

            return clone;
        }
    }
}