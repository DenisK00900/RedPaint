using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ImageView : PanelActive, IDrawable
    {
        private const float ZoomStep = 1f;
        private const float MinZoom = -3f;
        private const float MaxZoom = 3f;
        private const float LerpFactor = 0.08f;
        private const float VisualOffsetY = 32f;

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Vector2 innerPos;
        public float currScale = 0f;
        public float targetScale = 0f;

        private ChopTex chopCanvas;
        private ChopTex chopImage;

        public bool isTaken = false;
        private Vector2 takePos;

        private Sprite spriteCanvas;
        private Sprite spriteImage;

        public int MouseOverPosX;
        public int MouseOverPosY;

        private float timeMouseIn = 0f;
        private float timeMouseNeed = 0.4f;

        public ImageView(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            mc._image.ImageLoaded += UpdateImage;

            innerPos = Vector2.Zero;
            chopCanvas = new ChopTex(mc);
            chopImage = new ChopTex(mc);

            visual = new VisualElement[3];
            spriteCanvas = new Sprite(this);
            spriteImage = new Sprite(this);
            visual[0] = spriteCanvas;
            visual[1] = spriteImage;

            visual[2] = new Text(this);

            (visual[2] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[2] as Text).text = "000 000";
            visual[2].color = mc._settings.GetCurrPalletre().textColor1;

            UpdateImage();
        }

        public override AbstrEntity Clone() => throw new NotImplementedException();

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Изображение";
            depth = pl.baseRect.depth + 2;
        }

        public void UpdateImage()
        {
            innerPos = Vector2.Zero;
            currScale = 0f;
            targetScale = 0f;

            spriteCanvas.texture = mc._image.GetCanvas();
            spriteImage.texture = mc._image.GetCurrentImage();
        }

        public override void OnDestroy()
        {
            if (mc._image != null)
                mc._image.ImageLoaded -= UpdateImage;
        }

        private void UpdateChop()
        {
            chopCanvas.SourceTexture = mc._image.GetCanvas();
            chopImage.SourceTexture = mc._image.GetCurrentImage();

            var canvasSize = TUH.GetTextureSize(chopCanvas.SourceTexture);
            var imageSize = TUH.GetTextureSize(chopImage.SourceTexture);
            var halfOutline = panel.outlineSize / 2f;
            var centerOffset = activeRect.size / 2f + innerPos - halfOutline;

            var canvasPosition = centerOffset - canvasSize / 2f;
            var imagePosition = centerOffset - imageSize / 2f;

            var mainRect = new Rect(Vector2.Zero, activeRect.size - panel.outlineSize);

            float multiplier = 0.5f * (currScale - 1f);
            mainRect.position = canvasSize * multiplier;

            mainRect.size += canvasSize - (canvasSize * currScale);

            Rect canvasRect = new Rect(canvasPosition, canvasSize);
            Rect imageRect = new Rect(imagePosition, canvasSize);

            chopCanvas.cropMargins = TUH.CalculateCrop(mainRect, canvasRect);
            chopCanvas.cropMargins *= 1f / currScale;

            chopImage.cropMargins = TUH.CalculateCrop(mainRect, imageRect);
            chopImage.cropMargins *= 1f / currScale;

            chopCanvas.Generate();
            chopImage.Generate();

            spriteCanvas.texture = chopCanvas.Tex;
            spriteImage.texture = chopImage.Tex;
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

        private void UpdateMouseCoord(float deltaTime)
        {
            if (activeRect.CheckPoint(mc._input.GetMousePosition())) 
                timeMouseIn = Math.Clamp(timeMouseIn + deltaTime, 0f, timeMouseNeed);
            else 
                timeMouseIn = Math.Clamp(timeMouseIn - deltaTime, 0f, timeMouseNeed);

            Vector2 centerOffset = activeRect.size / 2f + innerPos - panel.outlineSize / 2f;

            centerOffset -= TUH.GetTextureSize(spriteCanvas);

            MouseOverPosX = (int)Math.Round(centerOffset.X);
            MouseOverPosY = (int)Math.Round(centerOffset.Y);

            visual[2].alpha = timeMouseIn / timeMouseNeed;
            (visual[2] as Text).text = MouseOverPosX.ToString() + " " + MouseOverPosY.ToString();
            (visual[2] as Text).pos =
                new Vector2(
                    (visual[2] as Text).GetRectSize().X / 2f + panel.outlineSize.X + 4f,
                    activeRect.size.Y + (visual[2] as Text).GetRectSize().Y - panel.outlineSize.Y / 2f
                    );
        }

        private void UpdateVisualElements()
        {
            var commonPos = activeRect.size / 2f + innerPos + new Vector2(0f, VisualOffsetY);

            spriteCanvas.pos = commonPos;
            spriteImage.pos = commonPos;
            spriteCanvas.scale = new Vector2(currScale);
            spriteImage.scale = new Vector2(currScale);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdateMovement();

            if (isTaken)
                innerPos = mc._input.GetMousePosition() - takePos;

            UpdateChop();
            UpdateMouseCoord(deltaTime);
            UpdateVisualElements();
        }
    }
}