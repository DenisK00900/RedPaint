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

        public void UpdateImage()
        {
            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc._image.GetCurrentImage();
        }

        public override void OnDestroy()
        {
            if (mc._image != null)
                mc._image.ImageLoaded -= UpdateImage;
        }

        public ImageViev(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[0];

            mc._image.ImageLoaded += UpdateImage;
        }
    }
}
