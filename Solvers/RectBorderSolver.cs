using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public static class RectBorderSolver
    {
        public static string GetBorder(Rect rect, Vector2 pos, float borderSize = 30f)
        {
            if (!rect.CheckPoint(pos))
                return "Out";

            bool isLeft = pos.X <= rect.position.X + borderSize;
            bool isRight = pos.X >= rect.position.X + rect.size.X - borderSize;
            bool isUp = pos.Y <= rect.position.Y + borderSize;
            bool isDown = pos.Y >= rect.position.Y + rect.size.Y - borderSize;

            if (isUp && isLeft) return "UpLeft";
            if (isUp && isRight) return "UpRight";
            if (isDown && isLeft) return "DownLeft";
            if (isDown && isRight) return "DownRight";

            if (isUp) return "Up";
            if (isDown) return "Down";
            if (isLeft) return "Left";
            if (isRight) return "Right";

            return "In";
        }

        public static Rect MaxRect(Rect main, Rect curr, List<Rect> closed, string borderToChange)
        {
            float left = curr.position.X;
            float right = curr.position.X + curr.size.X;
            float top = curr.position.Y;
            float bottom = curr.position.Y + curr.size.Y;

            if (string.IsNullOrEmpty(borderToChange) || borderToChange == "In" || borderToChange == "Out")
                return new Rect(new Vector2(left, top), new Vector2(right - left, bottom - top));

            bool canMoveLeft = borderToChange.Contains("Left");
            bool canMoveRight = borderToChange.Contains("Right");
            bool canMoveTop = borderToChange.Contains("Up");
            bool canMoveBottom = borderToChange.Contains("Down");

            var obstacles = closed?.Where(r => r != null && !ReferenceEquals(r, curr)).ToList() ?? new List<Rect>();

            if (canMoveLeft)
            {
                float minLeft = main.position.X;
                foreach (var obs in obstacles)
                {
                    if (top < obs.position.Y + obs.size.Y && bottom > obs.position.Y)
                    {
                        if (obs.position.X + obs.size.X <= left)
                        {
                            minLeft = Math.Max(minLeft, obs.position.X + obs.size.X);
                        }
                    }
                }
                left = minLeft;
            }

            if (canMoveRight)
            {
                float maxRight = main.position.X + main.size.X;
                foreach (var obs in obstacles)
                {
                    if (top < obs.position.Y + obs.size.Y && bottom > obs.position.Y)
                    {
                        if (obs.position.X >= right)
                        {
                            maxRight = Math.Min(maxRight, obs.position.X);
                        }
                    }
                }
                right = maxRight;
            }

            if (canMoveTop)
            {
                float minTop = main.position.Y;
                foreach (var obs in obstacles)
                {
                    if (left < obs.position.X + obs.size.X && right > obs.position.X)
                    {
                        if (obs.position.Y + obs.size.Y <= top)
                        {
                            minTop = Math.Max(minTop, obs.position.Y + obs.size.Y);
                        }
                    }
                }
                top = minTop;
            }

            if (canMoveBottom)
            {
                float maxBottom = main.position.Y + main.size.Y;
                foreach (var obs in obstacles)
                {
                    if (left < obs.position.X + obs.size.X && right > obs.position.X)
                    {
                        if (obs.position.Y >= bottom)
                        {
                            maxBottom = Math.Min(maxBottom, obs.position.Y);
                        }
                    }
                }
                bottom = maxBottom;
            }

            float width = Math.Max(0, right - left);
            float height = Math.Max(0, bottom - top);

            return new Rect(new Vector2(left, top), new Vector2(width, height));
        }
    }
}
