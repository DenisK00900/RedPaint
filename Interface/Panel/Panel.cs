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
    public class Panel : AbstrEntity, IReactToMouse
    {
        public Drawrect baseRect;
        public Drawrect outline;

        public Vector2 size;

        public Vector2 outlineSize = new Vector2(8, 8);
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public override void Update(float deltaTime)
        {
            (baseRect.visual[0] as Sprite).scale = size - outlineSize;
            (outline.visual[0] as Sprite).scale = size;

            base.Update(deltaTime);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);
        }

        public Panel(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);

            baseRect.position = outlineSize / 2f;

            outline.SetPos(baseRect.position - outlineSize);

            size = new Vector2(200, 400);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f);

            (outline.visual[0] as Sprite).origin = Vector2.Zero;
            (outline.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            outline.depth = baseRect.depth - 1;

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new List<Vector2>
                {
                    new Vector2(position.X, position.Y),
                    new Vector2(position.X + size.X, position.Y),
                    new Vector2(position.X + size.X, position.Y + size.Y),
                    new Vector2(position.X, position.Y + size.Y)
                });
            hb[0].parent = this;
        }
    }
}
