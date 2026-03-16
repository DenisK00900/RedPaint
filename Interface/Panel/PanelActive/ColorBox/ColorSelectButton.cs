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

namespace RedPaint
{
    public class ColorSelectButton : SpriteButton
    {
        private Vector2 outlineSize = new Vector2(4f, 4f);

        public float mouseOverTime = 0f;
        public float needTime = 0.25f;

        public Color origColor;
        public Color effColor;

        public float size = 32f;

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

            visual[2].color = Color.Lerp(origColor, effColor, mouseOverTime / needTime);

            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.75f);
        }

        public ColorSelectButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[3];

            visual[0] = new Sprite(this);
            visual[1] = new Sprite(this);
            visual[2] = new Sprite(this);

            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            visual[0].scale = new Vector2(size);
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.75f);

            (visual[1] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            visual[1].scale = new Vector2(size) - outlineSize;
            visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor1, 0.75f);

            (visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorHUE");
            visual[2].scale = new Vector2(size / 64f);

            origColor = mc._settings.GetCurrPalletre().textColor1;
            effColor = mc._settings.GetCurrPalletre().effectColor1;
        }
    }
}
