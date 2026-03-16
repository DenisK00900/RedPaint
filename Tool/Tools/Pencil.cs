using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Pencil : AbstrTool
    {
        private Vector2? lastPos = null;

        public int brushSize = 1;
        public Color brushColor = Color.Black;

        public Pencil(Maincode imc) : base(imc)
        {
            name = "Карандаш";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconPencil");
            dest = "Простой инструмент, который\nкрасит пиксели в определённый цвет";
        }

        public override void Update(float deltaTime)
        {
            brushColor = mc._image.GetColor();

            if (mc._input.IsDown(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    if (lastPos.HasValue)
                    {
                        DrawLine(lastPos.Value, texPos);
                    }
                    else
                    {
                        DrawBrush(texPos);
                    }

                    lastPos = texPos;
                }
            }
            else
            {
                lastPos = null;

                mc._image.ApplyChanges();
            }
        }

        private void DrawBrush(Vector2 pos)
        {
            int cx = (int)Math.Round(pos.X);
            int cy = (int)Math.Round(pos.Y);
            int radius = brushSize;

            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        mc._image.SetPixel(cx + dx, cy + dy, brushColor);
                    }
                }
            }
        }

        private void DrawLine(Vector2 start, Vector2 end)
        {
            int x0 = (int)Math.Round(start.X), y0 = (int)Math.Round(start.Y);
            int x1 = (int)Math.Round(end.X), y1 = (int)Math.Round(end.Y);

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                DrawBrush(new Vector2(x0, y0));

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private bool IsInTextureBounds(Vector2 pos)
        {
            var tex = mc._image.GetCurrentImage();
            return tex != null &&
                   pos.X >= 0 && pos.X < tex.Width &&
                   pos.Y >= 0 && pos.Y < tex.Height;
        }
        public void SetBrushSize(int size) => brushSize = Math.Max(1, size);
    }
}