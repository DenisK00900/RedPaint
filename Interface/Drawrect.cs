using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class Drawrect : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }

        public int depth { get; set; } = 0;

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override Drawrect Clone()
        {
            Drawrect clone = new Drawrect(mc);

            clone.SetPos(GetPos());
            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            clone.visual = ((IDrawable)this).CloneVisual();

            clone.depth = depth;

            return clone;
        }

        public Drawrect(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
        }
    }
}
