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
    public class RectSelect : AbstrTool
    {
        public BlockRender blockRender;
        public SelectBlockRender selectBlockRender;

        public bool isTaken = false;

        public bool isSolved = false;

        public Vector2 takenPos;

        Vector2 selectPos;

        public Rect takenRect;

        public RectSelect(Maincode imc) : base(imc)
        {
            name = "Выделение прямоугольником";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconRectSelect");

            dest = "Выделить указаную область\nпрямоугольником";

            blockRender = new BlockRender(mc);
            selectBlockRender = new SelectBlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            if (!isSolved)
            {
                if (isTaken)
                {
                    Vector2 currpos = GetTexPos();

                    Vector2 pos1 = new Vector2(Math.Min(takenPos.X, currpos.X), Math.Min(takenPos.Y, currpos.Y));
                    Vector2 pos2 = new Vector2(Math.Max(takenPos.X, currpos.X), Math.Max(takenPos.Y, currpos.Y));

                    blockRender.sizeX = (int)(Math.Max(pos2.X - pos1.X, 1f) * scale);
                    blockRender.sizeY = (int)(Math.Max(pos2.Y - pos1.Y, 1f) * scale);
                }
                else
                {
                    blockRender.sizeX = (int)(scale);
                    blockRender.sizeY = (int)(scale);
                }

                blockRender.thickness = 2f;

                blockRender.Generate();

                return blockRender.Tex;
            }
            else
            {
                selectBlockRender.sizeX = (int)(takenRect.size.X * scale);
                selectBlockRender.sizeY = (int)(takenRect.size.Y * scale);

                selectBlockRender.color1 = mc._settings.GetCurrPalletre().boxColor;
                selectBlockRender.color2 = mc._settings.GetCurrPalletre().effectColor2;

                selectBlockRender.thickness = 2f;

                selectBlockRender.Generate();

                return selectBlockRender.Tex;
            }
        }

        public override Vector2 GetAddPos(float scale = 1)
        {
            if (!isSolved)
            { 
                if (!isTaken) return base.GetAddPos(scale);

                Vector2 currpos = GetTexPos();

                return new Vector2(Math.Min(takenPos.X - currpos.X, 0), Math.Min(takenPos.Y - currpos.Y, 0)) * scale;
            }
            else
            {
                Vector2 currpos = GetTexPos();

                return new Vector2(takenRect.position.X - currpos.X, takenRect.position.Y - currpos.Y) * scale;
            }
        }

        public Texture2D GetCurrSelect()
        {
            Texture2D currentLayer = mc._image.GetCurrentImage();
            Color[] pixelBuffer = mc._image.GetPixelBuffer();

            int texWidth = currentLayer.Width;
            int texHeight = currentLayer.Height;
            int selectWidth = (int)takenRect.size.X;
            int selectHeight = (int)takenRect.size.Y;
            int startX = (int)takenRect.position.X;
            int startY = (int)takenRect.position.Y;

            Color[] selectedPixels = new Color[selectWidth * selectHeight];

            for (int y = 0; y < selectHeight; y++)
            {
                for (int x = 0; x < selectWidth; x++)
                {
                    int srcX = startX + x;
                    int srcY = startY + y;
                    int dstIndex = y * selectWidth + x;

                    if (srcX >= 0 && srcX < texWidth && srcY >= 0 && srcY < texHeight)
                    {
                        int srcIndex = srcY * texWidth + srcX;
                        selectedPixels[dstIndex] = pixelBuffer[srcIndex];
                    }
                    else
                    {
                        selectedPixels[dstIndex] = Color.Transparent;
                    }
                }
            }

            Texture2D SelectedRect = new Texture2D(mc.GraphicsDevice, selectWidth, selectHeight);
            SelectedRect.SetData(selectedPixels);

            return SelectedRect;
        }

        public void SetRect()
        {
            Vector2 currpos = GetTexPos();

            Vector2 pos1 = new Vector2(Math.Min(takenPos.X, currpos.X), Math.Min(takenPos.Y, currpos.Y));
            Vector2 pos2 = new Vector2(Math.Max(takenPos.X, currpos.X), Math.Max(takenPos.Y, currpos.Y));

            int sizeX = (int)(Math.Max(pos2.X - pos1.X, 1f));
            int sizeY = blockRender.sizeY = (int)(Math.Max(pos2.Y - pos1.Y, 1f));

            takenRect = new Rect(pos1, new Vector2(sizeX, sizeY));

            mc._select.SetTexture(GetCurrSelect());
            mc._select.pos = takenRect.position;
            mc._select.RemoveTexture();
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            selectBlockRender.AdvanceCycle();

            if (mc._input.IsPressed(Button.RightButton) && isSolved)
            {
                isSolved = false;

                mc._select.ApplyTexture();
            }

            if (isSolved)
            {
                if (mc._input.IsPressed(Button.LeftButton))
                {
                    isTaken = true;

                    takenPos = GetTexPos();
                    selectPos = mc._select.pos;
                }
                if (mc._input.IsReleased(Button.LeftButton))
                {
                    isTaken = false;
                }

                if (isTaken)
                {
                    mc._select.pos = selectPos + (GetTexPos() - takenPos);

                    takenRect.position = mc._select.pos;
                }
            }
            else
            {
                if (mc._input.IsPressed(Button.LeftButton))
                {
                    isTaken = true;
                    takenPos = GetTexPos();
                }

                if (mc._input.IsReleased(Button.LeftButton))
                {
                    isTaken = false;

                    Vector2 currpos = GetTexPos();

                    Vector2 pos1 = new Vector2(Math.Min(takenPos.X, currpos.X), Math.Min(takenPos.Y, currpos.Y));
                    Vector2 pos2 = new Vector2(Math.Max(takenPos.X, currpos.X), Math.Max(takenPos.Y, currpos.Y));

                    isSolved = true;

                    SetRect();
                }
            }
        }
    }
}
