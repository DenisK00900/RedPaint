using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ImageView : PanelActive, IDrawable
    {
        private const float ZoomStep = 1f;
        private const float MinZoom = -3f;
        private const float MaxZoom = 6f;
        private const float LerpFactor = 0.12f;
        private const float VisualOffsetY = 32f;

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Vector2 innerPos;
        public float currScale = 0f;
        public float targetScale = 0f;

        public bool isTaken = false;
        private Vector2 takePos;

        private Sprite spriteCanvas;

        public int MouseOverPosX;
        public int MouseOverPosY;

        private float timeMouseIn = 0f;
        private float timeMouseNeed = 0.4f;

        private Drawrect[] border = new Drawrect[4];

        public ImageView(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            mc._image.ImageLoaded += UpdateImage;

            innerPos = Vector2.Zero;

            visual = new VisualElement[3];
            spriteCanvas = new Sprite(this);

            visual[0] = spriteCanvas;

            visual[1] = new Text(this);

            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[1] as Text).text = "000 000";
            visual[1].color = mc._settings.GetCurrPalletre().textColor1;

            visual[2] = new Sprite(this);
            (visual[2] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/pixelselect");
            visual[2].origin = new Vector2(0f, 0f);

            border[0] = new Drawrect(mc, this);
            border[1] = new Drawrect(mc, this);
            border[2] = new Drawrect(mc, this);
            border[3] = new Drawrect(mc, this);

            UpdateImage();
        }

        public override AbstrEntity Clone() => throw new NotImplementedException();

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Изображение";
            depth = pl.baseRect.depth + 2;

            panel.baseRect.visual[0].isActive = false;

            panel.outline.visual[0].color = panel.baseRect.visual[0].color;
            panel.outline.depth = -3;
        }

        public void UpdateImage()
        {
            innerPos = Vector2.Zero;
            currScale = 0f;
            targetScale = 0f;

            spriteCanvas.texture = mc._image.GetCanvas();
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (visual == null) return;

            List<VisualElement> LayersVisual = new List<VisualElement>();

            for(int i = 0; i < mc._image.layers.Count; i++)
            {
                Sprite spr = new Sprite(this);

                spr.texture = mc._image.layers[i].tex;

                spr.pos = activeRect.size / 2f + innerPos + new Vector2(0f, VisualOffsetY);
                spr.scale = new Vector2(currScale);

                LayersVisual.Add(spr);
            }

            var device = mc.GraphicsDevice;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            sb.End();

            sb.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.NonPremultiplied,
                    SamplerState.PointClamp,
                    null,
                    null
                    );

            visual[0].Draw(sb);

            foreach (VisualElement item in LayersVisual)
            {
                item.Draw(sb);
            }

            visual[1].Draw(sb);
            visual[2].Draw(sb);

            sb.End();

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;

            sb.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null
            );
        }

        public override void OnDestroy()
        {
            if (mc._image != null)
                mc._image.ImageLoaded -= UpdateImage;
        }

        private void UpdateMovement()
        {
            if (activeRect.CheckPoint(mc._input.GetMousePosition()))
            {

                if (mc._input.IsMouseWheelScrolledUp())
                    targetScale = Math.Clamp(targetScale + ZoomStep, MinZoom, MaxZoom);
                else if (mc._input.IsMouseWheelScrolledDown())
                    targetScale = Math.Clamp(targetScale - ZoomStep, MinZoom, MaxZoom);

                if (mc._input.IsPressed(Button.MiddleButton) && !isTaken)
                {
                    isTaken = true;
                    takePos = mc._input.GetMousePosition() - innerPos;
                }
            }
            if (mc._input.IsReleased(Button.MiddleButton))
            {
                isTaken = false;
            }

            currScale = MathHelper.Lerp(currScale, MathF.Pow(2,targetScale), LerpFactor);
        }

        public Vector2 GetCurrTexPos()
        {
            return new Vector2(MouseOverPosX, MouseOverPosY);
        }

        private void UpdateSelectPixel()
        {
            if (activeRect.CheckPoint(mc._input.GetMousePosition()))
            {
                visual[2].isActive = true;
                


                if (false && TUH.GetPixelColor(mc._image.GetCurrentImage(), GetCurrTexPos()) != null)
                {
                    visual[2].color =
                        TUH.GetBrightness(TUH.GetPixelColor(mc._image.GetCurrentImage(), GetCurrTexPos()).Value) < 0.5f ?
                        Color.White : Color.Black;
                }
                else
                {
                    visual[2].color = Color.White;
                }

                visual[2].pos = (GetCurrTexPos() - TUH.GetTextureSize(spriteCanvas.texture)/2f) * currScale + innerPos + activeRect.size * 0.5f + new Vector2(0f,32f);
            }
            else
            {
                visual[2].isActive = false;
                visual[2].pos = Vector2.Zero;
            }

            visual[2].scale = new Vector2(currScale / 64f);
        }

        private void UpdateMouseCoord(float deltaTime)
        {
            if (activeRect.CheckPoint(mc._input.GetMousePosition())) 
                timeMouseIn = Math.Clamp(timeMouseIn + deltaTime, 0f, timeMouseNeed);
            else 
                timeMouseIn = Math.Clamp(timeMouseIn - deltaTime, 0f, timeMouseNeed);

            Vector2 mousePos = mc._input.GetMousePosition();
            Vector2 textureSize = TUH.GetTextureSize(visual[0] as Sprite);
            Vector2 scaleVector = Vector2.One * currScale;

            Vector2 centerOffset = (
                mousePos
                - activeRect.position
                - activeRect.size * 0.5f
                - scaleVector * 0.5f
                + textureSize * 0.5f
                - innerPos
            ) / currScale;

            MouseOverPosX = (int)Math.Round(centerOffset.X);
            MouseOverPosY = (int)Math.Round(centerOffset.Y);

            visual[1].alpha = timeMouseIn / timeMouseNeed;
            (visual[1] as Text).text = MouseOverPosX.ToString() + " " + MouseOverPosY.ToString();
            (visual[1] as Text).pos =
                new Vector2(
                    (visual[1] as Text).GetRectSize().X / 2f + panel.outlineSize.X + 4f,
                    activeRect.size.Y + (visual[1] as Text).GetRectSize().Y - panel.outlineSize.Y / 2f
                    );
        }

        private void UpdateVisualElements()
        {
            var commonPos = activeRect.size / 2f + innerPos + new Vector2(0f, VisualOffsetY);

            spriteCanvas.pos = commonPos;
            spriteCanvas.scale = new Vector2(currScale);
        }

        public override void OnSpawn()
        {
            for (int i = 0; i < 4;  i++)
            {
                mc._entityManager.AddEntity(border[i]);
            }
        }
        private void UpdateBorder()
        {
            border[0].visual[0].scale = new Vector2(panel.size.X, panel.outlineSize.Y/2f);
            border[0].visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            border[0].visual[0].origin = Vector2.Zero;
            border[0].visual[0].pos = new Vector2(0, 0);
            border[0].depth = -1;

            border[1].visual[0].scale = new Vector2(panel.size.X, panel.outlineSize.Y / 2f);
            border[1].visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            border[1].visual[0].origin = Vector2.Zero;
            border[1].visual[0].pos = new Vector2(0, panel.size.Y - panel.outlineSize.Y/2f);
            border[1].depth = -1;

            border[2].visual[0].scale = new Vector2(panel.outlineSize.X / 2f, panel.size.Y);
            border[2].visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            border[2].visual[0].origin = Vector2.Zero;
            border[2].visual[0].pos = new Vector2(0, 0);
            border[2].depth = -1;

            border[3].visual[0].scale = new Vector2(panel.outlineSize.X / 2f, panel.size.Y);
            border[3].visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);
            border[3].visual[0].origin = Vector2.Zero;
            border[3].visual[0].pos = new Vector2(panel.size.X - panel.outlineSize.X / 2f, 0);
            border[3].depth = -1;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdateMovement();

            if (isTaken)
                innerPos = mc._input.GetMousePosition() - takePos;

            UpdateMouseCoord(deltaTime);
            UpdateSelectPixel();
            UpdateVisualElements();

            depth = -2;

            UpdateBorder();
        }
    }
}