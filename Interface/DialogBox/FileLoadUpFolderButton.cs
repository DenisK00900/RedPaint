using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class FileLoadUpFolderButton : SpriteButton
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

        public FileLoadUpFolderButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            visual[1] = new Sprite(this);

            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.45f);
            visual[0].scale = new Vector2(32f, 32f);

            (visual[1] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/folderback");
            visual[1].color =
                mc._settings.GetCurrPalletre().textColor1;
            visual[1].scale = new Vector2(0.5f, 0.5f);

            origColor = mc._settings.GetCurrPalletre().textColor1;
            effColor = mc._settings.GetCurrPalletre().effectColor2;
        }
    }
}
