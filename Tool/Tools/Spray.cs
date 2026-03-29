using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Spray : AbstrTool
    {
        public CircleRender circleRender;
        public BlockRender blockRender;

        private Color sprayColor = Color.Black;
        private Random random = new Random();

        public int brushSize = 10;
        public int density = 20;
        public bool sqrBrush = false;

        public Spray(Maincode imc) : base(imc)
        {
            name = "Разбрызгиватель";

            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconSpray");
            dest = "Инструмент для создания эффекта разбрызгивания:\nзакрашивает случайные пиксели в области";

            circleRender = new CircleRender(mc);
            blockRender = new BlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            if (!sqrBrush)
            {
                circleRender.size = (int)(scale * Math.Max(1, (brushSize - 1) * 2f));
                circleRender.thickness = 2f;
                circleRender.Generate();
                return circleRender.Tex;
            }
            else
            {
                blockRender.size = (int)(scale * ((brushSize - 1) * 2f + 1));
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
            brushSizeSlider.minV = 1;
            brushSizeSlider.maxV = 100;
            brushSizeSlider.SetDef(10);

            ToolSetSliderInt densitySlider = new ToolSetSliderInt(mc);
            densitySlider.name = "Плотность";
            densitySlider.minV = 1;
            densitySlider.maxV = 200;
            densitySlider.SetDef(20);

            ToolSetBool squareBrush = new ToolSetBool(mc);
            squareBrush.name = "Квадратная кисть";

            NewClone(brushSizeSlider);
            NewClone(densitySlider);
            NewClone(squareBrush);

            sets.Add(brushSizeSlider);
            sets.Add(densitySlider);
            sets.Add(squareBrush);

            return sets;
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            sprayColor = mc._image.GetColor();
            brushSize = GetValue<int>("Размер кисти");
            density = GetValue<int>("Плотность");
            sqrBrush = GetValue<bool>("Квадратная кисть");

            if (mc._input.IsDown(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    SprayPixels(texPos);
                    mc._image.ApplyChanges();
                }
            }
        }

        private void SprayPixels(Vector2 center)
        {
            int cx = (int)Math.Round(center.X);
            int cy = (int)Math.Round(center.Y);
            int radius = brushSize - 1;

            for (int i = 0; i < density; i++)
            {
                int dx, dy;

                if (sqrBrush)
                {
                    dx = random.Next(-radius, radius + 1);
                    dy = random.Next(-radius, radius + 1);
                }
                else
                {
                    double angle = random.NextDouble() * Math.PI * 2;
                    double r = Math.Sqrt(random.NextDouble()) * radius;
                    dx = (int)Math.Round(Math.Cos(angle) * r);
                    dy = (int)Math.Round(Math.Sin(angle) * r);
                }

                int px = cx + dx;
                int py = cy + dy;

                if (IsInTextureBounds(new Vector2(px, py)))
                {
                    mc._image.SetPixel(px, py, sprayColor);
                }
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