using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RedPaint
{
    public class Text : VisualElement
    {
        public IDrawable parent { get; set; } = null;

        public Vector2 pos { get; set; } = Vector2.Zero;

        public float rotation { get; set; } = 0;

        public Vector2 scale { get; set; } = Vector2.One;

        public bool isAbsolute { get; set; } = false;

        public Vector2? origin { get; set; } = null;

        public Color color { get; set; } = Color.White;

        public float alpha { get; set; } = 1f;

        public bool isActive { get; set; } = true;

        public SpriteFont font { get; set; }

        public string text { get; set; } = "";

        public int index { get; set; }

        public Vector2 GetRectSize()
        {
            return font.MeasureString(text);
        }

        public void Draw(SpriteBatch sb)
        { 
            if (!isActive) return;

            if (text != "" && TUH.InsideScreen(pos, parent.mc._data, Math.Max(font.MeasureString(text).X, font.MeasureString(text).Y)))
            {
                Vector2 lorigin = origin.HasValue ? origin.Value : font.MeasureString(text) / 2f;

                sb.DrawString(
                    font,
                    text,
                    parent.GetPos() + pos,
                    color * alpha,
                    MathHelper.ToRadians(rotation),
                    lorigin,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public Text(IDrawable pr)
        {
            parent = pr;

            Random random = new Random();

            index = random.Next(0, 9999999);
        }
    }
}
