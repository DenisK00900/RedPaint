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
        private const float MinZoom = -4f;
        private const float MaxZoom = 8f;
        private const float LerpFactor = 0.08f;
        private const float VisualOffsetY = 32f;

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Vector2 innerPos;
        public float currScale = 0f;
        public float targetScale = 0f;

        public ChopTex chopCanvas;
        public ChopTex chopImage;

        public bool isTaken = false;
        private Vector2 takePos;

        private Sprite spriteCanvas;
        private Sprite spriteImage;

        public ImageView(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            mc._image.ImageLoaded += UpdateImage;

            innerPos = Vector2.Zero;
            chopCanvas = new ChopTex(mc);
            chopImage = new ChopTex(mc);

            visual = new VisualElement[2];
            spriteCanvas = new Sprite(this);
            spriteImage = new Sprite(this);
            visual[0] = spriteCanvas;
            visual[1] = spriteImage;

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
            Texture2D canvasTex = mc._image.GetCanvas();
            Texture2D imageTex = mc._image.GetCurrentImage();

            if (canvasTex != null) chopCanvas.SourceTexture = 
                TUH.ScaleTextureGPU(mc.GraphicsDevice, canvasTex, currScale, SamplerState.PointClamp);

            if (imageTex != null) chopImage.SourceTexture = 
                TUH.ScaleTextureGPU(mc.GraphicsDevice, imageTex, currScale, SamplerState.PointClamp);

            var canvasSize = TUH.GetTextureSize(chopCanvas.SourceTexture);
            var imageSize = TUH.GetTextureSize(chopImage.SourceTexture);
            var halfOutline = panel.outlineSize / 2f;
            var centerOffset = activeRect.size / 2f + innerPos - halfOutline;

            var scaledCanvasSize = canvasSize;
            var scaledImageSize = imageSize;

            var canvasPosition = centerOffset - scaledCanvasSize / 2f;
            var imagePosition = centerOffset - scaledImageSize / 2f;

            var mainRect = new Rect(activeRect.size - panel.outlineSize);

            Rect canvasRect = new Rect(canvasPosition, canvasSize);

            chopCanvas.cropMargins = TUH.CalculateCrop(mainRect, canvasRect);

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

            currScale = MathHelper.Lerp(currScale, MathF.Pow(2f, targetScale), LerpFactor);
        }

        private void UpdateVisualElements()
        {
            var commonPos = activeRect.size / 2f + innerPos + new Vector2(0f, VisualOffsetY);

            spriteCanvas.pos = commonPos;
            spriteImage.pos = commonPos;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdateMovement();

            if (isTaken)
                innerPos = mc._input.GetMousePosition() - takePos;

            UpdateChop();
            UpdateVisualElements();
        }
    }
}