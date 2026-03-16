using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace RedPaint
{
    public class SimpleButton : SpriteButton
    {
        public float mouseOverTime = 0f;
        public float needTime = 0.25f;

        public Color origColor;
        public Color effColor;

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

            if (visual[1] != null)
            {
                visual[1].color = Color.Lerp(origColor, effColor, mouseOverTime / needTime);
            }

            base.Update(deltaTime);
        }

        public override void OnSpawn()
        {
            UpdateHitbox();
        }

        public Vector2 GetSize()
        {
            return new Vector2((visual[1] as Text).GetRectSize().X + 8f, 40f);
        }

        public void Resize()
        {
            visual[0].scale = GetSize();
        }

        public SimpleButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().baseColor2, 0.75f);

            visual[1] = new Text(this);
            (visual[1] as Text).text = "Кнопка";
            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");

            visual[0].scale = GetSize();

            origColor = mc._settings.GetCurrPalletre().textColor1;
            effColor = mc._settings.GetCurrPalletre().effectColor1;
        }
    }
}
