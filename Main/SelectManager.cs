using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Dynamic;

namespace RedPaint
{
    public class SelectManager
    {
        Maincode mc;

        public Texture2D tex = null;

        public Vector2 pos = Vector2.Zero;

        public void SetTexture(Texture2D newtex)
        {
            tex = newtex;
        }

        public void ApplyTexture()
        {
            if (tex == null) return;

            var imageManager = mc._image;
            if (!imageManager.CanWrite()) return;

            if (imageManager.HasUnappliedChanges())
                imageManager.ApplyChanges();

            Texture2D currentLayer = imageManager.GetCurrentImage();
            Color[] pixelBuffer = imageManager.GetPixelBuffer();

            if (currentLayer == null || pixelBuffer == null) return;

            int texWidth = tex.Width;
            int texHeight = tex.Height;
            int startX = (int)pos.X;
            int startY = (int)pos.Y;
            int canvasWidth = currentLayer.Width;
            int canvasHeight = currentLayer.Height;

            Color[] texPixels = new Color[texWidth * texHeight];
            tex.GetData(texPixels);

            bool wasModified = false;

            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    int canvasX = startX + x;
                    int canvasY = startY + y;

                    if (canvasX < 0 || canvasX >= canvasWidth || canvasY < 0 || canvasY >= canvasHeight)
                        continue;

                    int bufferIndex = canvasY * canvasWidth + canvasX;
                    int texIndex = y * texWidth + x;

                    Color foreground = texPixels[texIndex];

                    if (foreground.A <= 0) continue;

                    Color background = pixelBuffer[bufferIndex];

                    float alpha = foreground.A / 255f;
                    Color result = new Color(
                        (byte)Math.Clamp(background.R * (1f - alpha) + foreground.R * alpha, 0, 255),
                        (byte)Math.Clamp(background.G * (1f - alpha) + foreground.G * alpha, 0, 255),
                        (byte)Math.Clamp(background.B * (1f - alpha) + foreground.B * alpha, 0, 255),
                        (byte)Math.Clamp(background.A * (1f - alpha) + foreground.A, 0, 255)
                    );

                    if (pixelBuffer[bufferIndex] != result)
                    {
                        pixelBuffer[bufferIndex] = result;
                        wasModified = true;
                    }
                }
            }

            if (wasModified)
            {
                currentLayer.SetData(pixelBuffer);

                imageManager.ApplyChanges();
                imageManager.isModified = true;
            }
        }

        public void RemoveTexture()
        {
            if (tex == null) return;

            var imageManager = mc._image;
            if (!imageManager.CanWrite()) return;

            if (imageManager.HasUnappliedChanges())
                imageManager.ApplyChanges();

            Texture2D currentLayer = imageManager.GetCurrentImage();
            Color[] pixelBuffer = imageManager.GetPixelBuffer();

            if (currentLayer == null || pixelBuffer == null) return;

            int texWidth = tex.Width;
            int texHeight = tex.Height;
            int startX = (int)pos.X;
            int startY = (int)pos.Y;
            int canvasWidth = currentLayer.Width;
            int canvasHeight = currentLayer.Height;

            Color[] selectPixels = new Color[texWidth * texHeight];
            tex.GetData(selectPixels);

            bool wasModified = false;

            for (int y = 0; y < texHeight; y++)
            {
                for (int x = 0; x < texWidth; x++)
                {
                    int canvasX = startX + x;
                    int canvasY = startY + y;

                    if (canvasX < 0 || canvasX >= canvasWidth || canvasY < 0 || canvasY >= canvasHeight)
                        continue;

                    int bufferIndex = canvasY * canvasWidth + canvasX;
                    int texIndex = y * texWidth + x;

                    if (selectPixels[texIndex].A > 0)
                    {
                        if (pixelBuffer[bufferIndex].A > 0)
                        {
                            pixelBuffer[bufferIndex] = Color.Transparent;
                            wasModified = true;
                        }
                    }
                }
            }

            if (wasModified)
            {
                currentLayer.SetData(pixelBuffer);

                imageManager.ApplyChanges();
                imageManager.isModified = true;
            }
        }

        public SelectManager(Maincode imc)
        {
            mc = imc;
        }
    }
}
