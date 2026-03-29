using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PaintBrush : AbstrTool
    {
        public CircleRender circleRender;

        private Vector2? lastPos = null;
        private Vector2? lastDirection = null;
        private float lastSpeed = 0f;

        public int brushSize = 20;
        public Color brushColor = Color.Black;
        public float flow = 0.8f;
        public float wetness = 0.5f;
        public float bristleSpread = 0.3f;
        public float directionInfluence = 0.7f;

        public PaintBrush(Maincode imc) : base(imc)
        {
            name = "Рисовальная кисть";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconPaintBrush");
            dest = "Реалистичная кисть с учётом направления движения,\nимитирующая поведение щетины";

            circleRender = new CircleRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            circleRender.size = (int)(scale * Math.Max(2, brushSize));
            circleRender.thickness = 0f;
            circleRender.Generate();

            return circleRender.Tex;
        }

        public override Vector2 GetAddPos(float scale = 1f)
        {
            return new Vector2(-brushSize * 0.5f * scale, -brushSize * 0.5f * scale);
        }

        public override List<ToolSet> GetSets()
        {
            follows.Clear();
            List<ToolSet> sets = new List<ToolSet>();

            var sizeSlider = new ToolSetSliderInt(mc)
            {
                name = "Размер кисти",
                minV = 5,
                maxV = 100
            };
            sizeSlider.SetDef(20);

            var flowSlider = new ToolSetSliderFloat(mc)
            {
                name = "Поток краски"
            };
            flowSlider.SetDef(0.8f);

            var wetnessSlider = new ToolSetSliderFloat(mc)
            {
                name = "Влажность"
            };
            wetnessSlider.SetDef(0.5f);

            var spreadSlider = new ToolSetSliderFloat(mc)
            {
                name = "Разброс щетины"
            };
            spreadSlider.SetDef(0.3f);

            var directionSlider = new ToolSetSliderFloat(mc)
            {
                name = "Влн. направления"
            };
            directionSlider.SetDef(0.7f);

            var controls = new List<ToolSet>
            {
                sizeSlider, flowSlider, wetnessSlider, spreadSlider, directionSlider // Удалён opacitySlider
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
            flow = GetValue<float>("Поток краски");
            wetness = GetValue<float>("Влажность");
            bristleSpread = GetValue<float>("Разброс щетины");
            directionInfluence = GetValue<float>("Влн. направления");

            if (mc._input.IsDown(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    Vector2 direction = Vector2.Zero;
                    float speed = 0f;

                    if (lastPos.HasValue)
                    {
                        direction = texPos - lastPos.Value;
                        speed = direction.Length();

                        if (speed > 0.1f)
                        {
                            direction.Normalize();
                            lastDirection = direction;
                            lastSpeed = MathHelper.Lerp(lastSpeed, speed, 0.2f);
                        }
                    }

                    DrawBrushWithDirection(texPos, direction, speed);

                    lastPos = texPos;
                }
            }
            else
            {
                lastPos = null;
                lastDirection = null;
                lastSpeed = 0f;
                mc._image.ApplyChanges();
            }
        }

        private void DrawBrushWithDirection(Vector2 centerPos, Vector2 direction, float speed)
        {
            int cx = (int)Math.Round(centerPos.X);
            int cy = (int)Math.Round(centerPos.Y);
            int radius = brushSize / 2;

            float baseAlpha = flow * MathHelper.Clamp(1f - speed * 0.02f, 0.3f, 1f);

            int bristleCount = Math.Max(8, brushSize / 3);
            Random rng = new Random((int)(cx * 1000 + cy));

            for (int b = 0; b < bristleCount; b++)
            {
                float angleOffset = rng.NextSingle() * MathHelper.TwoPi;
                float spreadOffset = rng.NextSingle() * bristleSpread * radius;

                Vector2 bristleOffset = new Vector2(
                    MathF.Cos(angleOffset) * spreadOffset,
                    MathF.Sin(angleOffset) * spreadOffset
                );

                if (lastDirection.HasValue && directionInfluence > 0f)
                {
                    float dragAmount = MathHelper.Clamp(lastSpeed * 0.1f, 0f, radius * 0.5f) * directionInfluence;
                    bristleOffset += lastDirection.Value * dragAmount * rng.NextSingle();
                }

                int bx = cx + (int)Math.Round(bristleOffset.X);
                int by = cy + (int)Math.Round(bristleOffset.Y);

                DrawBristle(bx, by, radius * 0.6f, baseAlpha * (0.5f + rng.NextSingle() * 0.5f));
            }
        }

        private void DrawBristle(int cx, int cy, float radius, float alpha)
        {
            int r = (int)Math.Ceiling(radius);

            for (int dy = -r; dy <= r; dy++)
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    float dist = MathF.Sqrt(dx * dx + dy * dy);

                    if (dist <= radius)
                    {
                        int px = cx + dx;
                        int py = cy + dy;

                        if (!IsInTextureBounds(new Vector2(px, py))) continue;

                        float edgeAlpha = 1f - MathF.Pow(dist / radius, 1.5f);
                        float finalAlpha = alpha * edgeAlpha;

                        if (finalAlpha < 0.01f) continue;

                        Color original = mc._image.GetPixel(px, py);
                        Color result = BlendWithWetness(original, brushColor, finalAlpha, wetness);

                        mc._image.SetPixel(px, py, result);
                    }
                }
            }
        }

        private Color BlendWithWetness(Color background, Color foreground, float alpha, float wetness)
        {
            float dryFactor = 1f - wetness;

            Color blended = new Color(
                Math.Clamp(background.R * (1 - alpha) + foreground.R * alpha, 0, 255),
                (byte)Math.Clamp(background.G * (1 - alpha) + foreground.G * alpha, 0, 255),
                (byte)Math.Clamp(background.B * (1 - alpha) + foreground.B * alpha, 0, 255),
                255
            );

            if (wetness > 0f)
            {
                blended = new Color(
                    Math.Clamp(blended.R * dryFactor + background.R * wetness * 0.3f, 0, 255),
                    (byte)Math.Clamp(blended.G * dryFactor + background.G * wetness * 0.3f, 0, 255),
                    (byte)Math.Clamp(blended.B * dryFactor + background.B * wetness * 0.3f, 0, 255),
                    255
                );
            }

            return blended;
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