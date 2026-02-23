using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Reflection.Emit;
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
        public bool mouseOver { get; set; } = false;

        public bool isTaken = false;

        public Rect targetRect = null;

        public Rect lastRect = null;

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }
        
        public void SetRectAsPos(Rect rect)
        {
            position = rect.position;
            size = rect.size;
        }

        public Rect GetRect()
        {
            return new Rect(position, size);
        }

        public void UpdateHitbox()
        {
            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(lastRect);
            hb[0].depth = baseRect.depth;
            hb[0].parent = this;
        }

        public override void Update(float deltaTime)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (isTaken)
            {
                targetRect = mc.mainHolder.GetRectUnder(mousePosition);

                if (targetRect == null) targetRect = lastRect;

                if (mc._input.IsReleased(Button.LeftButton))
                {
                    isTaken = false;

                    mc.mainHolder.AddPanel(this, targetRect);
                }
            }
            else
            {
                if (mc._input.IsPressed(Button.LeftButton) && mouseOver)
                {
                    isTaken = true;

                    mc.mainHolder.DeletePanel(this);
                }
            }

            position = TUH.Lerp(position, targetRect.position, 0.1f);
            size = TUH.Lerp(size, targetRect.size, 0.1f);

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
            isAbsolute = true;

            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);

            baseRect.position = outlineSize / 2f;

            outline.SetPos(baseRect.position - outlineSize);

            size = new Vector2(1, 1);

            targetRect = lastRect = GetRect();

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f);

            (outline.visual[0] as Sprite).origin = Vector2.Zero;
            (outline.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            outline.depth = baseRect.depth - 1;

            UpdateHitbox();
        }
    }
}
