using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Rect
    {
        public Vector2 position;

        public Vector2 size;

        public Rect Clone()
        {
            return new Rect(position, size);
        }

        public bool CheckPoint(Vector2 point)
        {
            return 
                point.X >= position.X && 
                point.Y >= position.Y &&
                point.X <= position.X + size.X &&
                point.Y <= position.Y + size.Y;
        }

        public Rect GetSubrect(int w, int h, int x, int y)
        {
            if (w <= 0 || h <= 0) throw new ArgumentException("...");
            float subWidth = size.X / w;
            float subHeight = size.Y / h;
            return new Rect(
                new Vector2(position.X + x * subWidth, position.Y + y * subHeight),
                new Vector2(subWidth, subHeight));
        }

        public bool CollideWith(Rect other)
        {
            return
                position.X < other.position.X + other.size.X &&
                position.X + size.X > other.position.X &&
                position.Y < other.position.Y + other.size.Y &&
                position.Y + size.Y > other.position.Y;
        }

        public override string ToString()
        {
            return $"Rect: {position.X} - {position.Y}, {size.X} - {size.Y}";
        }

        public Vector2 Center => position + size * 0.5f;

        public void SetBorder(string border, Vector2 pos, Vector2 minSize, Rect maxRect = null)
        {
            if (maxRect != null)
            {
                if (maxRect.size.X < minSize.X)
                    throw new ArgumentException(
                        $"maxRect width ({maxRect.size.X}) is smaller than minSize.X ({minSize.X})");
                if (maxRect.size.Y < minSize.Y)
                    throw new ArgumentException(
                        $"maxRect height ({maxRect.size.Y}) is smaller than minSize.Y ({minSize.Y})");
            }

            if (border.Contains("Up"))
            {
                float newY = pos.Y;

                if (maxRect != null)
                {
                    newY = Math.Max(newY, maxRect.position.Y);
                }

                float currentBottom = position.Y + size.Y;
                float maxAllowedY = currentBottom - minSize.Y;
                newY = Math.Min(newY, maxAllowedY);

                newY = Math.Min(newY, currentBottom);

                float deltaY = position.Y - newY;
                size.Y += deltaY;
                position.Y = newY;
            }
            else if (border.Contains("Down"))
            {
                float newBottom = pos.Y;

                if (maxRect != null)
                {
                    newBottom = Math.Min(newBottom, maxRect.position.Y + maxRect.size.Y);
                }

                float minAllowedBottom = position.Y + minSize.Y;
                newBottom = Math.Max(newBottom, minAllowedBottom);

                newBottom = Math.Max(newBottom, position.Y);

                size.Y = newBottom - position.Y;
            }

            if (border.Contains("Left"))
            {
                float newX = pos.X;

                if (maxRect != null)
                {
                    newX = Math.Max(newX, maxRect.position.X);
                }

                float currentRight = position.X + size.X;
                float maxAllowedX = currentRight - minSize.X;
                newX = Math.Min(newX, maxAllowedX);

                newX = Math.Min(newX, currentRight);

                float deltaX = position.X - newX;
                size.X += deltaX;
                position.X = newX;
            }
            else if (border.Contains("Right"))
            {
                float newRight = pos.X;

                if (maxRect != null)
                {
                    newRight = Math.Min(newRight, maxRect.position.X + maxRect.size.X);
                }

                float minAllowedRight = position.X + minSize.X;
                newRight = Math.Max(newRight, minAllowedRight);

                newRight = Math.Max(newRight, position.X);

                size.X = newRight - position.X;
            }

            if (size.X < minSize.X) size.X = minSize.X;
            if (size.Y < minSize.Y) size.Y = minSize.Y;
        }

        public void SetBorder(string border, Vector2 pos, Rect maxRect = null)
        {
            if (border.Contains("Up"))
            {
                float newY = pos.Y;

                if (maxRect != null)
                {
                    newY = Math.Max(newY, maxRect.position.Y);
                }
                newY = Math.Min(newY, position.Y + size.Y);

                float deltaY = position.Y - newY;
                size.Y += deltaY;
                position.Y = newY;
            }
            else if (border.Contains("Down"))
            {
                float newBottom = pos.Y;

                if (maxRect != null)
                {
                    newBottom = Math.Min(newBottom, maxRect.position.Y + maxRect.size.Y);
                }
                newBottom = Math.Max(newBottom, position.Y);

                size.Y = newBottom - position.Y;
            }

            if (border.Contains("Left"))
            {
                float newX = pos.X;

                if (maxRect != null)
                {
                    newX = Math.Max(newX, maxRect.position.X);
                }
                newX = Math.Min(newX, position.X + size.X);

                float deltaX = position.X - newX;
                size.X += deltaX;
                position.X = newX;
            }
            else if (border.Contains("Right"))
            {
                float newRight = pos.X;

                if (maxRect != null)
                {
                    newRight = Math.Min(newRight, maxRect.position.X + maxRect.size.X);
                }
                newRight = Math.Max(newRight, position.X);

                size.X = newRight - position.X;
            }

            if (size.X < 0) size.X = 0;
            if (size.Y < 0) size.Y = 0;
        }

        public Rect()
        {

        }

        public Rect(Vector2 pos, Vector2 s)
        {
            position = pos;
            size = s;
        }

        public Rect(Vector2 pos, Vector2 s, Vector2 origin)
        {
            size = s;

            position = pos - size * origin;
        }
    }
}
