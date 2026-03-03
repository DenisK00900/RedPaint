using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ImageViev : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);

            depth = pl.baseRect.depth + 1;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public ImageViev(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[0];
        }
    }
}
