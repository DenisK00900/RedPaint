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
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public Action action = null;

        private float stanCurr = 0f;
        public float stanHold = 0.05f;

        public override void Update(float deltaTime)
        {
            if (stanCurr == 0f && mc._input.IsPressed(Button.LeftButton) && mouseOver)
            {
                if (action != null) action.Act();

                stanCurr = stanHold;
            }
            
            stanCurr = Math.Clamp(stanCurr - deltaTime, 0f, stanHold);
        }

        public abstract void UpdateHitbox();

        public AbstrActButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
