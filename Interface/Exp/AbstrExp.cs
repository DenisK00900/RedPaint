using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public abstract class AbstrExp : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }

        public Hitbox[] hb {  get; set; }

        public int depth { get; set; } = 0;

        public bool mouseOver { get; set; } = false;

        public bool isExpanded { get; set; } = false;

        public virtual void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public abstract void Expand();

        public abstract void Collapse();

        public AbstrExp(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {

        }
    }
}
