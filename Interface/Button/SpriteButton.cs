using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RedPaint.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class SpriteButton : AbstrActButton, IDrawable, IHint
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override SpriteButton Clone()
        {
            SpriteButton clone = new SpriteButton(mc, parent);

            clone.visual = ((IDrawable)this).CloneVisual();

            clone.depth = depth;

            clone.hint = hint.Clone();

            return clone;
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

        public SpriteButton(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
