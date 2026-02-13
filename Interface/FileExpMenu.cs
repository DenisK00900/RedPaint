using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class FileExpMenu : ExpMenu
    {
        float mouseOverTime = 0f;
        float needTime = 0.25f;

        AbstrEntity follow;

        public override void Update(float deltaTime)
        {
            if (mouseOver)
            {
                mouseOverTime = Math.Clamp(mouseOverTime + deltaTime, 0f, needTime);

                if (TUH.GetMouseClick() == 0)
                {
                    Expand();
                }
            }
            else
            {
                mouseOverTime = Math.Clamp(mouseOverTime - deltaTime, 0f, needTime);

                if (TUH.GetMouseClick() == 1)
                {
                    Collapse();
                }
            }

            visual[0].color = Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, mouseOverTime/ needTime);
        }

        public override void Expand()
        {
            follow = new PopMenu(mc, Vector2.Zero, this);

            mc._settings.AddEntity(follow);
        }

        public override void Collapse()
        {
            follow.Destroy();
        }

        public FileExpMenu(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[1]; 

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).text = "Файл";
            (visual[0] as Text).color = mc._settings.GetCurrPalletre().textColor1;

            Vector2 box = (visual[0] as Text).GetRectSize()/2f;

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
