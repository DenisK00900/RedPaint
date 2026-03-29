using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace RedPaint
{
    public class ActionRotateImage : AbstrAction
    {
        public int rotation;

        public override void Act()
        {
            var imgManager = mc._image;
            if (imgManager == null || imgManager.layers.Count == 0) return;

            if (!imgManager.CanWrite()) return;

            int layerIdx = imgManager.workingLayer;
            var currentLayer = imgManager.layers[layerIdx];
            var sourceTex = currentLayer.tex;
            if (sourceTex == null) return;

            Color[] sourceData = new Color[sourceTex.Width * sourceTex.Height];
            sourceTex.GetData(sourceData);

            int srcWidth = sourceTex.Width;
            int srcHeight = sourceTex.Height;
            int dstWidth, dstHeight;

            if (rotation % 2 == 1)
            {
                dstWidth = srcHeight;
                dstHeight = srcWidth;
            }
            else
            {
                dstWidth = srcWidth;
                dstHeight = srcHeight;
            }

            Color[] destData = new Color[dstWidth * dstHeight];

            int normalizedRot = ((rotation % 4) + 4) % 4;

            for (int y = 0; y < srcHeight; y++)
            {
                for (int x = 0; x < srcWidth; x++)
                {
                    int srcIndex = y * srcWidth + x;
                    int dstX, dstY;

                    switch (normalizedRot)
                    {
                        case 0: // 0 градусов - без изменений
                            dstX = x;
                            dstY = y;
                            break;
                        case 1: // 90 градусов по часовой стрелке
                            dstX = y;
                            dstY = srcWidth - 1 - x;
                            break;
                        case 2: // 180 градусов
                            dstX = srcWidth - 1 - x;
                            dstY = srcHeight - 1 - y;
                            break;
                        case 3: // 270 градусов по часовой стрелке (90 против часовой)
                            dstX = srcHeight - 1 - y;
                            dstY = x;
                            break;
                        default:
                            dstX = x;
                            dstY = y;
                            break;
                    }

                    int dstIndex = dstY * dstWidth + dstX;
                    destData[dstIndex] = sourceData[srcIndex];
                }
            }

            Texture2D rotatedTex = new Texture2D(mc.GraphicsDevice, dstWidth, dstHeight);
            rotatedTex.SetData(destData);

            sourceTex.Dispose();

            imgManager.SetImage(rotatedTex, layerIdx);

            imgManager.isModified = true;

            imgManager.CallChanges();
        }

        public ActionRotateImage(Maincode imc, int rot = 0) : base(imc)
        {
            rotation = rot;
        }
    }
}
