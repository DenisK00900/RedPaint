using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Fill : AbstrTool
    {
        private Color fillColor = Color.Black;
        private int tolerance = 32;

        public Fill(Maincode imc) : base(imc)
        {
            name = "Заливка";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconFill");
            dest = "Закрашивает область в\nопределённый цвет";
        }

        public override List<ToolSet> GetSets()
        {
            follows.Clear();

            List<ToolSet> sets = new List<ToolSet>();

            ToolSetSliderInt toleranceSlider = new ToolSetSliderInt(mc);
            toleranceSlider.name = "Толерантность";

            toleranceSlider.minV = 0;
            toleranceSlider.maxV = 255;
            toleranceSlider.SetDef(32);

            NewClone(toleranceSlider);
            sets.Add(toleranceSlider);

            return sets;
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            fillColor = mc._image.GetColor();
            tolerance = GetValue<int>("Толерантность");

            if (mc._input.IsPressed(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    int startX = (int)Math.Round(texPos.X);
                    int startY = (int)Math.Round(texPos.Y);

                    FloodFill(startX, startY, fillColor, tolerance);
                    mc._image.ApplyChanges();
                }
            }
        }

        private void FloodFill(int startX, int startY, Color newColor, int tolerance)
        {
            var tex = mc._image.GetCurrentImage();
            if (tex == null) return;

            int width = tex.Width;
            int height = tex.Height;

            Color targetColor = mc._image.GetPixel(startX, startY);

            if (targetColor == newColor && tolerance == 0) return;

            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new HashSet<Point>();

            queue.Enqueue(new Point(startX, startY));
            visited.Add(new Point(startX, startY));

            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                int x = current.X;
                int y = current.Y;

                mc._image.SetPixel(x, y, newColor);

                for (int i = 0; i < 4; i++)
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        Point neighbor = new Point(nx, ny);

                        if (!visited.Contains(neighbor))
                        {
                            Color neighborColor = mc._image.GetPixel(nx, ny);

                            if (IsColorSimilar(neighborColor, targetColor, tolerance))
                            {
                                visited.Add(neighbor);
                                queue.Enqueue(neighbor);
                            }
                        }
                    }
                }
            }
        }

        private bool IsColorSimilar(Color c1, Color c2, int tolerance)
        {
            return Math.Abs(c1.R - c2.R) <= tolerance &&
                   Math.Abs(c1.G - c2.G) <= tolerance &&
                   Math.Abs(c1.B - c2.B) <= tolerance &&
                   Math.Abs(c1.A - c2.A) <= tolerance;
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