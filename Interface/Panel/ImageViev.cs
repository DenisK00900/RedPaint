using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RedPaint;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ImageViev : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public Vector2 InnerPos;

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);

            depth = pl.baseRect.depth + 1;
        }

        public void UpdateImage()
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc._image.GetCanvas();

            visual[1] = new Sprite(this);
            (visual[1] as Sprite).texture = mc._image.GetCurrentImage();
        }

        public override void OnDestroy()
        {
            if (mc._image != null)
                mc._image.ImageLoaded -= UpdateImage;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (visual.Length >= 1) visual[0].pos = activeRect.size / 2f + InnerPos;
            if (visual.Length >= 2) visual[1].pos = activeRect.size / 2f + InnerPos;
        }

        public ImageViev(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[0];

            mc._image.ImageLoaded += UpdateImage;

            InnerPos = Vector2.Zero;
        }
    }
}
