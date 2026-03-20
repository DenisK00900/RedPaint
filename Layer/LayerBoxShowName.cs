using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RedPaint.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class LayerBoxShowName : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public void UpdateHitbox()
        {
            Vector2 textSize = (visual[0] as Text).GetRectSize();

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(textSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos() - (visual[0] as Text).GetRectSize()/2f;
        }

        public override void Update(float deltaTime)
        {
            if (mouseOver && mc._input.IsPressed(Button.LeftButton))
            {
                (parent.parent as LayerBox).SetThisLayer();
            }

            base.Update(deltaTime);
        }

        public LayerBoxShowName(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).text = "Новый слой";

            visual[0].color = mc._settings.GetCurrPalletre().textColor1;
        }
    }
}
