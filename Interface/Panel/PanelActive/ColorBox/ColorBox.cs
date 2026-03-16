using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ColorBox : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public ColorSelectButton HueSelectButton;
        public ColorSelectButton WheelSelectButton;
        public ColorSelectButton RGBSelectButton;

        public Drawrect uppanel;

        public AbstrEntity colorSelect = null;

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Палитра";
            SetDepth(pl.baseRect.depth + 2);

            HueSelectButton.SetPos(
                new Vector2(HueSelectButton.size*0.5f, HueSelectButton.size*0.5f) +
                new Vector2(0f, 32f) + panel.outlineSize/2f);

            WheelSelectButton.SetPos(
                new Vector2(WheelSelectButton.size * 0.5f, WheelSelectButton.size * 0.5f) +
                new Vector2(HueSelectButton.size, 32f) + panel.outlineSize / 2f);

            RGBSelectButton.SetPos(
                new Vector2(RGBSelectButton.size * 0.5f, RGBSelectButton.size * 0.5f) +
                new Vector2(HueSelectButton.size + WheelSelectButton.size, 32f) + panel.outlineSize / 2f);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(HueSelectButton);
            mc._entityManager.AddEntity(WheelSelectButton);
            mc._entityManager.AddEntity(RGBSelectButton);

            mc._entityManager.AddEntity(uppanel);

            mc._entityManager.AddEntity(colorSelect);

            base.OnSpawn();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            HueSelectButton.UpdateHitbox();
            WheelSelectButton.UpdateHitbox();
            RGBSelectButton.UpdateHitbox();

            uppanel.SetPos(new Vector2(
                HueSelectButton.size + WheelSelectButton.size + RGBSelectButton.size, 32f) + panel.outlineSize/2f);
            uppanel.visual[0].scale = new Vector2(
                activeRect.size.X -
                (HueSelectButton.size + WheelSelectButton.size + RGBSelectButton.size) -
                panel.outlineSize.X,
                HueSelectButton.size);

            colorSelect.SetPos(activeRect.size/2f + new Vector2(30f, 32f + HueSelectButton.size/2f));

            if (colorSelect is HUEColorBox h)
            {
                h.SetSize(activeRect.size +
                    new Vector2(-60f,- HueSelectButton.size) -
                    panel.outlineSize
                    );

                h.SetAlphaSize(
                    new Vector2(30f, activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y));

                h.alpha.SetPos(new Vector2(- activeRect.size.X / 2f + panel.outlineSize.X/2f,
                    -(activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y)/2f));

                h.SetColorSize(
                    new Vector2(30f, activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y));

                h.color.SetPos(new Vector2(-activeRect.size.X / 2f + panel.outlineSize.X / 2f - 30f,
                    -(activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y) / 2f));
            }
        }

        public override void SetDepth(int depth)
        {
            base.SetDepth(depth);

            HueSelectButton.SetDepth(depth + 3);
            WheelSelectButton.SetDepth(depth + 3);
            RGBSelectButton.SetDepth(depth + 3);

            uppanel.SetDepth(depth + 2);

            colorSelect.SetDepth(depth + 1);
        }

        public ColorBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            colorSelect = new HUEColorBox(mc, this);

            uppanel = new Drawrect(mc, this);
            uppanel.visual[0].origin = Vector2.Zero;
            uppanel.visual[0].color = 
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().baseColor2, 0.9f);

            HueSelectButton = new ColorSelectButton(mc, this);
            WheelSelectButton = new ColorSelectButton(mc, this);
            RGBSelectButton = new ColorSelectButton(mc, this);

            (HueSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorHUE");
            (WheelSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorWheel");
            (RGBSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorRGB");
        }
    }
}
