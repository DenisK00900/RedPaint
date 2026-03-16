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
    public abstract class AbstrActButton : AbstrEntity, IReactToMouse
    {
        public float mouseOverStopTime { get; set; } = 0f;
        public float mouseOverStopTimeNeed { get; set; } = 1.0f;
        public bool hintShow { get; set; } = false;
        public Hint hint { get; set; } = null;

        public Hint hintclone = null;

        public bool chainBlock = false;

        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public List<AbstrAction> action = new List<AbstrAction>();

        private float stanCurr = 0f;
        public float stanHold = 0.05f;

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
                if (chainBlock)
                {
                    foreach (AbstrAction item in action)
                    {
                        item.Call();

                        if (!item.succCall) break;
                    }
                }
                else
                {
                    foreach (AbstrAction item in action)
                    {
                        item.Act();
                    }
                }

                stanCurr = stanHold;
            }
            
            stanCurr = Math.Clamp(stanCurr - deltaTime, 0f, stanHold);
        }

        public void SetHintText(string text)
        {
            hint.message = text;
        }

        public void AddAction(AbstrAction ia)
        {
            action.Add(ia);
        }

        public override void OnDestroy()
        {
            hint.Destroy();
            if (hintclone != null) hintclone.Destroy();

            base.OnDestroy();
        }

        public abstract void UpdateHitbox();

        public AbstrActButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            hint = new Hint(mc);
        }
    }
}
