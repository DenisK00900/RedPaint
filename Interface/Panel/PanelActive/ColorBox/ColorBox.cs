using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ColorBox : PanelActive
    {
        public ColorSelectButton HueSelectButton;
        public ColorSelectButton WheelSelectButton;
        public ColorSelectButton RGBSelectButton;

        public Drawrect uppanel;

        public AbstrEntity colorSelect = null;

        public float alphaPoint = 1.0f;
        public float colorPoint = 1.0f;

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

                h.alpha.SetPos(new Vector2(- activeRect.size.X / 2f + panel.outlineSize.X/2f - 30f,
                    -(activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y)/2f) +
                    h.alpha.size/2f);

                h.SetColorSize(
                    new Vector2(30f, activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y));

                h.color.SetPos(new Vector2(-activeRect.size.X / 2f + panel.outlineSize.X / 2f,
                    -(activeRect.size.Y - HueSelectButton.size - panel.outlineSize.Y) / 2f) +
                    h.color.size / 2f);

                h.UpdateHitbox();
            }

            (uppanel.visual[1] as Text).text = 
                $"R:{mc._image.GetColor().R} G:{mc._image.GetColor().G} B:{mc._image.GetColor().B} A:{mc._image.GetColor().A}";
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

            uppanel.visual = new VisualElement[2];

            uppanel.visual[0] = new Sprite(uppanel);
            (uppanel.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            uppanel.visual[0].origin = Vector2.Zero;
            uppanel.visual[0].color = 
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().baseColor2, 0.9f);

            uppanel.visual[1] = new Text(uppanel);
            (uppanel.visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (uppanel.visual[1] as Text).text = "R:255 G:255 B:255 A:255";
            uppanel.visual[1].origin = Vector2.Zero;
            uppanel.visual[1].pos = new Vector2(8f, 8f);
            uppanel.visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.5f);

            HueSelectButton = new ColorSelectButton(mc, this);
            WheelSelectButton = new ColorSelectButton(mc, this);
            RGBSelectButton = new ColorSelectButton(mc, this);

            (HueSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorHUE");
            (WheelSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorWheel");
            (RGBSelectButton.visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/ColorRGB");
        }
    }
}
