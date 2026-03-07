using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class DelayMenuElement : AbstrEntity, IDrawable, IMenuElement
    {
        public override DelayMenuElement Clone()
        {
            DelayMenuElement clone = new DelayMenuElement(mc, parent);

            return clone;
        }

        public DelayMenuElement(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Text(this);

            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.75f);
            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).text = "=---O---=";
            visual[0].isAbsolute = true;
        }

        public bool saveParent { get; set; }
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public Vector2 GetSize()
        {
            return (visual[0] as Text).GetRectSize();
        }

        public void SetElementDepth(int depth)
        {
            SetDepth(depth);
        }

        public void SetElementPos(Vector2 pos)
        {
            visual[0].pos = pos;
        }
    }
}
