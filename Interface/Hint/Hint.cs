using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Diagnostics;

namespace RedPaint
{
    public class Hint : AbstrEntity, IDrawable
    {
        public string message = "Для данного элемента\nнет доступной подсказки";

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public Drawrect baseRect;
        public Drawrect outline;

        public Vector2 outlineSize = new Vector2(8f, 8f);

        public override Hint Clone()
        {
            Hint clone = new Hint(mc, message, parent);

            return clone;
        }

        public override void OnSpawn()
        {
            SetPos(GetPos() + ((visual[0] as Text).GetRectSize() + new Vector2(8f)) / 2f + new Vector2(16f));

            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);
        }

        public override void SetDepth(int depth)
        {
            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);

            base.SetDepth(depth + 2);
        }

        public Hint(Maincode imc, string text = null, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            if (text != null) message = text;

            visual[0] = new Text(this);
            (visual[0] as Text).text = message;
            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            visual[0].color = mc._settings.GetCurrPalletre().textColor1;

            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);

            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f),
            mc._settings.GetCurrPalletre().boxColor, 0.10f);

            (outline.visual[0] as Sprite).color =
            Color.Lerp(
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f),
            mc._settings.GetCurrPalletre().boxColor, 0.10f);

            Vector2 size = (visual[0] as Text).GetRectSize() + new Vector2(8f);

            baseRect.visual[0].scale = size;
            outline.visual[0].scale = size + outlineSize;
        }
    }
}
