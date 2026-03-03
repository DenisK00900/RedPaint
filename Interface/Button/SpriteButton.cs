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
    public class SpriteButton : AbstrActButton, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public override void UpdateHitbox()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(texSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos() - texSize / 2f;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public SpriteButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
