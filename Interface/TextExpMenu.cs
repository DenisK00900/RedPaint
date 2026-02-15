using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class TextExpMenu : ExpMenu
    {
        public Text text;

        public override void Update(float deltaTime)
        {
            color = Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, mouseOverTime / needTime);

            base.Update(deltaTime);
        }

        public override TextExpMenu Clone()
        {
            TextExpMenu clone = new TextExpMenu(mc, text, prototape);

            clone.SetPos(GetPos());
            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            clone.visual = ((IDrawable)this).CloneVisual();

            clone.text = text;
            clone.mouseOverTime = mouseOverTime;
            clone.needTime = needTime;
            clone.color = color;
            clone.hb = hb;
            clone.depth = depth;

            return clone;
        }

        public TextExpMenu(Maincode mc, Text tx, AbstrEntity cr, AbstrEntity pr = null) : base(mc, cr, pr)
        {
            visual = new VisualElement[1];

            text = tx;
            text.parent = this;

            visual[0] = text;

            Vector2 box = (visual[0] as Text).GetRectSize() / 2f;

            hb = new Hitbox[1];

            hb[0] = new PolygonHitbox(new List<Vector2>
            {
                new Vector2(-box.X, -box.Y),
                new Vector2(box.X, -box.Y),
                new Vector2(box.X, box.Y),
                new Vector2(-box.X, box.Y)
            });
            hb[0].parent = this;
        }
    }
}
