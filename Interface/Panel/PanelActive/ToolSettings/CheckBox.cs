using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RedPaint.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class CheckBox : AbstrActButton, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        private Texture2D onIcon;

        private Texture2D offIcon;

        public List<AbstrAction> offAction = new List<AbstrAction>();

        public List<AbstrAction> onAction = new List<AbstrAction>();

        public bool status = false;

        private void ChangeIcon()
        {
            (visual[0] as Sprite).texture = status ? onIcon : offIcon;
        }

        public override void UpdateHitbox()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(texSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos() - texSize / 2f;
        }

        public override void Update(float deltaTime)
        {
            if (mouseOver)
            {
                if (mc._input.IsMouseMoved() && !hintShow)
                {
                    mouseOverStopTime = 0f;
                }
                else
                {
                    mouseOverStopTime += deltaTime;
                }
            }
            else
            {
                mouseOverStopTime = 0f;
            }

            if (mouseOverStopTime >= mouseOverStopTimeNeed && !hintShow)
            {
                hintShow = true;
                hintclone = hint.Clone();
                hintclone.position = mc._input.GetMousePosition();
                hintclone.SetDepth(999);
                mc._entityManager.AddEntity(hintclone);
            }
            else if (mouseOverStopTime < mouseOverStopTimeNeed && hintShow)
            {
                mouseOverStopTime = 0f;
                hintShow = false;
                hintclone.Destroy();
            }

            if (stanCurr == 0f && mc._input.IsPressed(Button.LeftButton) && mouseOver)
            {
                status = !status;

                if (!status)
                {
                    if (chainBlock)
                    {
                        foreach (AbstrAction item in offAction)
                        {
                            item.Call();

                            if (!item.succCall) break;
                        }
                    }
                    else
                    {
                        foreach (AbstrAction item in offAction)
                        {
                            item.Act();
                        }
                    }
                }
                else
                {
                    if (chainBlock)
                    {
                        foreach (AbstrAction item in onAction)
                        {
                            item.Call();

                            if (!item.succCall) break;
                        }
                    }
                    else
                    {
                        foreach (AbstrAction item in onAction)
                        {
                            item.Act();
                        }
                    }
                }

                stanCurr = stanHold;

                ChangeIcon();
            }

            stanCurr = Math.Clamp(stanCurr - deltaTime, 0f, stanHold);
        }

        public CheckBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Sprite(this);

            onIcon = mc.Content.Load<Texture2D>("Texture/Icons/CheckBoxOn");
            offIcon = mc.Content.Load<Texture2D>("Texture/Icons/CheckBoxOff");

            (visual[0] as Sprite).color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            ChangeIcon();
        }
    }
}
