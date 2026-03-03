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
    public class DialogBox : AbstrEntity
    {
        public Drawrect baseRect;
        public Drawrect outline;
        public DialogBoxSetRect setRect;

        public bool isSetPanel = true;

        bool isTaken = false;

        Vector2 takenPos = Vector2.Zero;

        public Vector2 outlineSize = new Vector2(8, 8);

        public override DialogBox Clone()
        {
            DialogBox clone = new DialogBox(mc, parent);

            return clone;
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);

            if (isSetPanel) mc._entityManager.AddEntity(setRect);
        }

        public virtual Vector2 DetermentSize()
        {
            return new Vector2(600f, 450f);
        }

        public void SetOnCenter()
        {
            baseRect.SetPos(mc._data.res/2f - baseRect.visual[0].scale/2f);
        }

        public virtual void OnTake()
        {

        }

        public virtual void OnDrop()
        {

        }

        public override void Update(float deltaTime)
        {
            if (!isTaken)
            {
                if (mc._input.IsPressed(Button.LeftButton) && setRect.mouseOver)
                {
                    if (TUH.GetHitboxCollideIndex(setRect.hb, mc._input.GetMousePosition()) == 0)
                    {
                        isTaken = true;

                        takenPos = mc._input.GetMousePosition() - GetPos();

                        OnTake();
                    }
                    else if (TUH.GetHitboxCollideIndex(setRect.hb, mc._input.GetMousePosition()) == 1)
                    {
                        Close();
                    }
                }
            }
            else
            {
                SetPos(mc._input.GetMousePosition() - takenPos);

                if (mc._input.IsReleased(Button.LeftButton))
                {
                    isTaken = false;

                    OnDrop();
                }
            }
        }

        public virtual void Close()
        {
            Destroy();
        }

        public DialogBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);
            setRect = new DialogBoxSetRect(mc, baseRect);

            baseRect.depth += 3;
             
            Vector2 size = DetermentSize();

            (baseRect.visual[0] as Sprite).scale = size - outlineSize;
            (outline.visual[0] as Sprite).scale = size;
            (setRect.visual[0] as Sprite).scale = new Vector2(size.X - outlineSize.X, 32);

            (setRect.visual[0] as Sprite).origin = Vector2.Zero;
            (setRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.60f);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.85f);

            (outline.visual[0] as Sprite).origin = Vector2.Zero;
            (outline.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.35f);
            outline.depth = baseRect.depth - 1;
            outline.SetPos(-outlineSize/2);

            setRect.depth = baseRect.depth + 1;

            SetOnCenter();
        }
    }
}
