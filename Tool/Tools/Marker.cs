using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Marker : AbstrTool
    {
        public CircleRender circleRender;
        public BlockRender blockRender;

        private Vector2? lastPos = null;
        private float lastPressure = 1f;

        public int brushSize = 12;
        public Color brushColor = Color.Yellow;
        public bool sqrBrush = false;
        public bool chiselTip = false; 
        public float chiselAngle = 45f;
        public bool inkAccumulation = true;

        public Marker(Maincode imc) : base(imc)
        {
            name = "Маркер";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconMarker");
            dest = "Полупрозрачный маркер с накоплением чернил и скошенным кончиком";

            circleRender = new CircleRender(mc);
            blockRender = new BlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            int size = sqrBrush || chiselTip
                ? (int)(scale * ((brushSize - 1) * 2f + 1))
                : (int)(scale * Math.Max(1, (brushSize - 1) * 2f));

            if (!sqrBrush && !chiselTip)
            {
                circleRender.size = size;
                circleRender.thickness = 0f;
                circleRender.Generate();
                return circleRender.Tex;
            }
            else
            {
                blockRender.sizeX = size;
                blockRender.sizeY = size;
                blockRender.thickness = 0f;
                blockRender.Generate();
                return blockRender.Tex;
            }
        }

        public override Vector2 GetAddPos(float scale = 1f)
        {
            int offset = chiselTip || sqrBrush ? (brushSize - 1) : (brushSize / 2);
            return new Vector2(-offset * scale, -offset * scale);
        }

        public override List<ToolSet> GetSets()
        {
            follows.Clear();
            List<ToolSet> sets = new List<ToolSet>();

            var sizeSlider = new ToolSetSliderInt(mc)
            {
                name = "Размер кисти",
                minV = 2,
                maxV = 60
            };
            sizeSlider.SetDef(12);

            var squareBrush = new ToolSetBool(mc) { name = "Квадратная кисть" };
            var chiselToggle = new ToolSetBool(mc) { name = "Скошенный кончик" };

            var chiselAngleSlider = new ToolSetSliderInt(mc)
            {
                name = "Угол скоса",
                minV = 0,
                maxV = 360
            };
            chiselAngleSlider.SetDef(45);

            var accumulationToggle = new ToolSetBool(mc) { name = "Накопление чернил" };

            var controls = new List<ToolSet>
            {
                sizeSlider, squareBrush, chiselToggle, chiselAngleSlider, accumulationToggle
            };

            foreach (var s in controls)
            {
                NewClone(s);
                sets.Add(s);
            }

            return sets;
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            brushColor = mc._image.GetColor();
            brushSize = GetValue<int>("Размер кисти");
            sqrBrush = GetValue<bool>("Квадратная кисть");
            chiselTip = GetValue<bool>("Скошенный кончик");
            chiselAngle = (float)GetValue<int>("Угол скоса");
            inkAccumulation = GetValue<bool>("Накопление чернил");

            float currentPressure = mc._input.IsDown(Button.LeftButton) ? 1f : 0f;
            float pressure = MathHelper.Lerp(lastPressure, currentPressure, 0.3f);
            lastPressure = pressure;

            if (mc._input.IsDown(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    Color finalColor = new Color(
                        brushColor.R, brushColor.G, brushColor.B,
                        (byte)(255 * pressure));

                    if (lastPos.HasValue)
                    {
                        DrawSmoothLine(lastPos.Value, texPos, finalColor, pressure);
                    }
                    else
                    {
                        DrawBrush(texPos, finalColor, pressure);
                    }
                    lastPos = texPos;
                }
            }
            else
            {
                lastPos = null;
                lastPressure = 1f;
                mc._image.ApplyChanges();
            }
        }

        private void DrawBrush(Vector2 pos, Color color, float pressure)
        {
            int cx = (int)Math.Round(pos.X);
            int cy = (int)Math.Round(pos.Y);
            int radius = brushSize - 1;

            float angleRad = MathHelper.ToRadians(chiselAngle);
            float cosA = MathF.Cos(angleRad), sinA = MathF.Sin(angleRad);

            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    bool inside = IsPointInBrush(dx, dy, radius, cosA, sinA);
                    if (!inside) continue;

                    int px = cx + dx;
                    int py = cy + dy;
                    if (!IsInTextureBounds(new Vector2(px, py))) continue;

                    float finalAlpha = pressure;
                    if (finalAlpha < 0.01f) continue;

                    Color original = mc._image.GetPixel(px, py);
                    Color result = inkAccumulation
                        ? BlendWithAccumulation(original, color, finalAlpha)
                        : BlendStandard(original, color, finalAlpha);

                    mc._image.SetPixel(px, py, result);
                }
            }
        }

        private bool IsPointInBrush(int dx, int dy, int radius, float cosA, float sinA)
        {
            if (sqrBrush)
                return Math.Abs(dx) <= radius && Math.Abs(dy) <= radius;

            if (chiselTip)
            {
                float rx = dx * cosA + dy * sinA;
                float ry = -dx * sinA + dy * cosA;
                float width = radius * 1.5f;
                float height = radius * 0.6f;
                return (rx * rx) / (width * width) + (ry * ry) / (height * height) <= 1f;
            }

            return dx * dx + dy * dy <= radius * radius;
        }

        private Color BlendWithAccumulation(Color background, Color foreground, float alpha)
        {
            float a = Math.Clamp(alpha, 0f, 1f);
            float bgA = background.A / 255f;
            float accumulationFactor = inkAccumulation ? (1f + bgA * 0.5f) : 1f;

            return new Color(
                (byte)Math.Clamp(background.R * (1 - a * accumulationFactor) + foreground.R * a * accumulationFactor, 0, 255),
                (byte)Math.Clamp(background.G * (1 - a * accumulationFactor) + foreground.G * a * accumulationFactor, 0, 255),
                (byte)Math.Clamp(background.B * (1 - a * accumulationFactor) + foreground.B * a * accumulationFactor, 0, 255),
                (byte)Math.Clamp(255 * (1 - (1 - bgA) * (1 - a)), 0, 255)
            );
        }

        private Color BlendStandard(Color background, Color foreground, float alpha)
        {
            float a = Math.Clamp(alpha, 0f, 1f);
            return new Color(
                Math.Clamp(background.R * (1 - a) + foreground.R * a, 0, 255),
                (byte)Math.Clamp(background.G * (1 - a) + foreground.G * a, 0, 255),
                (byte)Math.Clamp(background.B * (1 - a) + foreground.B * a, 0, 255),
                255
            );
        }

        private void DrawSmoothLine(Vector2 start, Vector2 end, Color color, float pressure)
        {
            Vector2 delta = end - start;
            float length = delta.Length();

            if (length < 0.5f)
            {
                DrawBrush(end, color, pressure);
                return;
            }

            int steps = Math.Max(1, (int)(length * 1.5f));
            Vector2 step = delta / steps;

            for (int i = 0; i <= steps; i++)
            {
                Vector2 pos = start + step * i;
                DrawBrush(pos, color, pressure);
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