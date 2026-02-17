using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public abstract class ExpMenu : AbstrExp
    {
        public float mouseOverTime = 0f;
        public float needTime = 0.25f;

        private AbstrEntity follow;

        public AbstrEntity prototape;

        public Color color;

        public override void Update(float deltaTime)
        {
            if (mouseOver)
            {
                mouseOverTime = Math.Clamp(mouseOverTime + deltaTime, 0f, needTime);

                if (TUH.GetMouseClick() == 0 && !isExpanded && prototape != null)
                {
                    Expand();
                }
            }
            else
            {
                mouseOverTime = Math.Clamp(mouseOverTime - deltaTime, 0f, needTime);

                if ((TUH.GetMouseClick() == 1 || TUH.GetMouseClick() == 0) && isExpanded)
                {
                    Collapse();
                }
            }

            foreach (VisualElement item in visual)
            {
                visual[0].color = color;
            }
        }

        public override void Expand()
        {
            follow = prototape.Clone();

            mc._entityManager.AddEntity(follow);

            isExpanded = true;
        }

        public override void Collapse()
        {
            follow.Destroy();

            follow = null;

            isExpanded = false;
        }

        public ExpMenu(Maincode mc, AbstrEntity cr, AbstrEntity pr = null) : base(mc, pr)
        {
            prototape = cr;
        }
    }
}
