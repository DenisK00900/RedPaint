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

        public override void Update(float deltaTime)
        {
            if (mc._input.IsPressed(Button.LeftButton) && mouseOver)
            {
                if (action != null) action.Act();
            }
        }

        public abstract void UpdateHitbox();

        public override void OnSpawn()
        {
            UpdateHitbox();
        }

        public AbstrActButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
