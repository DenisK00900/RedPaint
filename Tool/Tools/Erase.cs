using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Erase : AbstrTool
    {
        public BlockRender blockRender;
        public CircleRender circleRender;

        private Vector2? lastPos = null;

        public int brushSize = 1;
        public bool sqrBrush = false;

        public Erase(Maincode imc) : base(imc)
        {
            name = "Ластик";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconErase");
            dest = "Стирает цвет пикселя,\nвозвращая его к прозрачности\nили цвету фона";

            blockRender = new BlockRender(mc);
            circleRender = new CircleRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            if (!sqrBrush)
            {
                circleRender.size = (int)(scale * Math.Max(1, (int)((brushSize - 1) * 2f)));

                circleRender.thickness = 2f;

                circleRender.Generate();

                return circleRender.Tex;
            }
            else
            {
                blockRender.sizeX = (int)(scale * ((brushSize - 1) * 2f + 1));
                blockRender.sizeY = (int)(scale * ((brushSize - 1) * 2f + 1));

                blockRender.thickness = 2f;

                blockRender.Generate();

                return blockRender.Tex;
            }
        }

        public override Vector2 GetAddPos(float scale = 1f)
        {
            if (!sqrBrush)
            {
                return new Vector2(Math.Min(0, (1.5f - brushSize)) * scale);
            }
            else
            {
                return new Vector2(scale * -(brushSize - 1));
            }
        }
        public override List<ToolSet> GetSets()
        {
            follows.Clear();

            List<ToolSet> sets = new List<ToolSet>();

            ToolSetSliderInt brushSizeSlider = new ToolSetSliderInt(mc);
            brushSizeSlider.name = "Размер кисти";

            ToolSetBool squareBrush = new ToolSetBool(mc);
            squareBrush.name = "Квадратная кисть";

            NewClone(brushSizeSlider);
            NewClone(squareBrush);

            sets.Add(brushSizeSlider);
            sets.Add(squareBrush);

            return sets;
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            brushSize = GetValue<int>("Размер кисти");
            sqrBrush = GetValue<bool>("Квадратная кисть");

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
            int radius = brushSize - 1;

            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (sqrBrush || dx * dx + dy * dy <= radius * radius)
                    {
                        mc._image.SetPixel(cx + dx, cy + dy, Color.Transparent);
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
    }
}