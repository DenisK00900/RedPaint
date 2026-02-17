using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class TextExpMenu : ExpMenu, IMenuElement
    {
        public Text text;
        public bool saveParent { get; set; } = false;

        public override void Update(float deltaTime)
        {
            color = Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, mouseOverTime / needTime);

            base.Update(deltaTime);
        }

        public override TextExpMenu Clone()
        {
            Text clonedText = text.Clone() as Text;
            AbstrEntity clonedPrototape = prototape?.Clone();

            TextExpMenu clone = new TextExpMenu(mc, clonedText, clonedPrototape, parent);

            clone.SetPos(position);

            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            clone.mouseOverTime = mouseOverTime;
            clone.needTime = needTime;
            clone.color = color;
            clone.depth = depth;

            clonedText.parent = clone;

            return clone;
        }

        public void SetElementPos(Vector2 pos)
        {
            SetPos(pos);
        }

        public Vector2 GetSize() 
        {
            return text.GetRectSize();
        }

        public TextExpMenu(Maincode mc, Text tx, AbstrEntity cr, AbstrEntity pr = null) : base(mc, cr, pr)
        {
            visual = new VisualElement[1];

            text = tx;
            text.parent = this;

            visual[0] = text;

            Vector2 textSize = text.GetRectSize();
            Vector2 textOrigin = text.origin ?? (textSize / 2f);

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new List<Vector2>
                {
                    new Vector2(-textOrigin.X, -textOrigin.Y),
                    new Vector2(textSize.X - textOrigin.X, -textOrigin.Y),
                    new Vector2(textSize.X - textOrigin.X, textSize.Y - textOrigin.Y),
                    new Vector2(-textOrigin.X, textSize.Y - textOrigin.Y)
                });
            hb[0].parent = this;
        }
    }
}
