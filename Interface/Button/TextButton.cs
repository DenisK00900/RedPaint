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
    public class TextButton : AbstrActButton, IDrawable, IMenuElement
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public bool saveParent { get; set; } = false;

        public float mouseOverTime = 0f;
        public float needTime = 0.25f;

        public Color origColor;
        public Color effColor;

        public override TextButton Clone()
        {
            TextButton clone = new TextButton(mc, parent);
            clone.visual = (this as IDrawable).CloneVisual();
            clone.depth = depth;
            clone.saveParent = saveParent;

            clone.action = action;

            SendCloneTo(clone);

            return clone;
        }

        public void SetText(Text it)
        {
            visual = new VisualElement[1];
            visual[0] = it;
            visual[0].parent = this;

            UpdateHitbox();
        }

        public void SetHitboxPos(Vector2 pos)
        {
            hb[0].pos = pos;
        }

        public override void UpdateHitbox()
        {
            Vector2 textSize = (visual[0] as Text).GetRectSize();

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(textSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public override void Update(float deltaTime)
        {
            if (mouseOver)
            {
                mouseOverTime = Math.Clamp(mouseOverTime + deltaTime, 0f, needTime);
            }
            else
            {
                mouseOverTime = Math.Clamp(mouseOverTime - deltaTime, 0f, needTime);
            }

            if (visual[0] != null)
            {
                visual[0].color = Color.Lerp(origColor, effColor, mouseOverTime / needTime);
            }

            base.Update(deltaTime);
        }

        public Vector2 GetSize()
        {
            return (visual[0] as Text).GetRectSize();
        }

        public void SetElementPos(Vector2 pos)
        {
            visual[0].pos = pos;

            UpdateHitbox();

            SetHitboxPos(pos - (visual[0] as Text).GetRectSize() / 2f);
        }

        public void SetElementDepth(int depth)
        {
            hb[0].depth = depth;
        }

        public TextButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            origColor = mc._settings.GetCurrPalletre().textColor1;
            effColor = mc._settings.GetCurrPalletre().effectColor1;
        }
    }
}
