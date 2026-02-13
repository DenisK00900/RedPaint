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

        public Drawrect(Maincode mc, AbstrEntity pr = null) : base(pr)
        {
            this.mc = mc;

            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
        }
    }
}
