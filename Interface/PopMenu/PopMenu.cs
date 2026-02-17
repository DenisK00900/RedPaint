using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PopMenu : AbstrEntity
    {
        public Drawrect outline;
        public Drawrect baseRect;

        public Vector2 size;

        public Vector2 outlineSize = new Vector2(8, 8);

        public override void OnSpawn()
        {
            baseRect.visual[0].scale = size;
            outline.visual[0].scale = size + outlineSize;

            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);
        }

        public override PopMenu Clone()
        {
            PopMenu clone = new PopMenu(mc, position, parent);

            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            clone.size = size;
            clone.outlineSize = outlineSize;

            return clone;
        }

        public PopMenu(Maincode mc, Vector2 pos, AbstrEntity pr = null) : base(mc, pos, pr)
        {
            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);

            outline.SetPos(baseRect.position - outlineSize/2f);

            size = new Vector2(200, 400);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color = 
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f);
            (baseRect.visual[0] as Sprite).scale = size;

            (outline.visual[0] as Sprite).origin = Vector2.Zero;
            (outline.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            (outline.visual[0] as Sprite).scale = size + outlineSize;
            outline.depth = baseRect.depth - 1;
        }
    }
}
