using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RedPaint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public static class TUH
    {
        public static void PrintEntityHierarchy(Maincode mc)
        {
            if (mc == null || mc.entities == null)
                return;

            Debug.WriteLine("===== ENTITY HIERARCHY =====");

            var allEntities = new HashSet<AbstrEntity>(mc.entities);

            var rootEntities = new List<AbstrEntity>();
            foreach (var entity in mc.entities)
            {
                if (entity.parent == null || !allEntities.Contains(entity.parent))
                {
                    rootEntities.Add(entity);
                }
            }

            var processed = new HashSet<AbstrEntity>();
            PrintEntityHierarchyRecursive(rootEntities, 0, processed, allEntities);

            Debug.WriteLine("==========================");
        }

        private static void PrintEntityHierarchyRecursive(
            List<AbstrEntity> entities,
            int level,
            HashSet<AbstrEntity> processed,
            HashSet<AbstrEntity> allEntities)
        {
            string indent = new string(' ', level * 2);

            foreach (var entity in entities)
            {
                if (processed.Contains(entity))
                {
                    Debug.WriteLine(indent + $"{entity.GetType().Name} [CYCLIC REFERENCE]");
                    continue;
                }
                processed.Add(entity);

                string entityInfo = $"{entity.GetType().Name} [0x{entity.GetHashCode():X4}]";

                Vector2 pos = entity.GetPos();
                entityInfo += $" (Pos: {pos.X:F1}, {pos.Y:F1})";

                if (entity.markForDestroy)
                    entityInfo += " [MARKED FOR DESTROY]";

                Debug.WriteLine(indent + entityInfo);

                var validChildren = new List<AbstrEntity>();
                foreach (var child in entity.children)
                {
                    if (allEntities.Contains(child) && !processed.Contains(child))
                    {
                        validChildren.Add(child);
                    }
                }

                if (validChildren.Count > 0)
                {
                    PrintEntityHierarchyRecursive(validChildren, level + 1, processed, allEntities);
                }
            }
        }

        public static int GetMouseClick()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) return 0;
            else if (mouseState.RightButton == ButtonState.Pressed) return 1;
            else if (mouseState.MiddleButton == ButtonState.Pressed) return 2;

            return -1;
        }
        public static void SortByIndex(List<VisualElement> elements)
        {
            if (elements == null)
                return;

            elements.Sort((a, b) =>
            {
                return a.index.CompareTo(b.index);
            });
        }
        public static List<VisualElement> GetSortedElements(List<VisualElement> elements)
        {
            if (elements == null)
                return new List<VisualElement>();

            return elements.OrderBy(e => e.index).ToList();
        }
        public static Vector2 CalculateAveragePoint(List<Vector2> points)
        {
            return CalculateAveragePoint(points.ToArray());
        }
        public static Vector2 CalculateAveragePoint(Vector2[] points)
        {
            if (points == null || points.Length == 0)
                throw new ArgumentException("Array cannot be null or empty");

            float totalX = 0f;
            float totalY = 0f;

            foreach (var point in points)
            {
                totalX += point.X;
                totalY += point.Y;
            }

            return new Vector2(totalX / points.Length, totalY / points.Length);
        }
        public static Vector2 RotatePoint(Vector2 pointToRotate, float angleInDegrees)
        {
            float angleInRadians = MathHelper.ToRadians(angleInDegrees);

            float cosTheta = (float)Math.Cos(angleInRadians);
            float sinTheta = (float)Math.Sin(angleInRadians);

            Vector2 rotatedPoint;
            rotatedPoint.X = pointToRotate.X * cosTheta - pointToRotate.Y * sinTheta;
            rotatedPoint.Y = pointToRotate.X * sinTheta + pointToRotate.Y * cosTheta;

            return rotatedPoint;
        }
        public static string GetTimeInFormat(float time, string format = "M", bool show_micro = false)
        {
            float totalUnits = time;
            int days = 0, hours = 0, minutes = 0, seconds = 0;

            switch (format)
            {
                case "D":
                    days = (int)(time / 86400);
                    hours = (int)((time % 86400) / 3600);
                    minutes = (int)((time % 3600) / 60);
                    seconds = (int)(time % 60);
                    break;

                case "H":
                    hours = (int)(time / 3600);
                    minutes = (int)((time % 3600) / 60);
                    seconds = (int)(time % 60);
                    break;

                case "M":
                    minutes = (int)(time / 60);
                    seconds = (int)(time % 60);
                    break;

                case "S":
                default:
                    seconds = (int)time;
                    break;
            }

            string result;
            switch (format)
            {
                case "D":
                    result = show_micro ? $"{days:D2}:{hours:D2}:{minutes:D2}:{seconds:D2}" : $"{days:D2}:{hours:D2}:{minutes:D2}:{seconds:D2}";
                    break;

                case "H":
                    result = show_micro ? $"{hours:D2}:{minutes:D2}:{seconds:D2}" : $"{hours:D2}:{minutes:D2}:{seconds:D2}";
                    break;

                case "M":
                    result = show_micro ? $"{minutes:D2}:{seconds:D2}" : $"{minutes:D2}:{seconds:D2}";
                    break;

                case "S":
                default:
                    result = show_micro ? seconds.ToString() : seconds.ToString();
                    break;
            }

            if (show_micro && format != "S")
            {
                float fractionalPart = time % 1;
                int milliseconds = (int)(fractionalPart * 1000);
                result += $".{milliseconds:D3}";
            }
            else if (show_micro && format == "S")
            {
                float fractionalPart = time % 1;
                int milliseconds = (int)(fractionalPart * 1000);
                result = $"{seconds}.{milliseconds:D3}";
            }

            return result;
        }
        public static Color Lerp(Color color1, Color color2, float amount)
        {
            amount = MathHelper.Clamp(amount, 0f, 1f);

            byte r = (byte)(color1.R + (color2.R - color1.R) * amount);
            byte g = (byte)(color1.G + (color2.G - color1.G) * amount);
            byte b = (byte)(color1.B + (color2.B - color1.B) * amount);
            byte a = (byte)(color1.A + (color2.A - color1.A) * amount);

            return new Color(r, g, b, a);
        }
        public static T[] ConcatArrays<T>(T[] firstArray, T[] secondArray)
        {
            if (firstArray == null || secondArray == null)
            {
                throw new ArgumentNullException("Массивы не могут быть null.");
            }

            T[] result = new T[firstArray.Length + secondArray.Length];

            Array.Copy(firstArray, result, firstArray.Length);
            Array.Copy(secondArray, 0, result, firstArray.Length, secondArray.Length);

            return result;
        }
        public static Texture2D[] LoadAsAnim(ContentManager content, string path)
        {
            List<Texture2D> textures = new List<Texture2D>();
            int index = 1;

            while (true)
            {
                try
                {
                    Texture2D texture = content.Load<Texture2D>(path + index.ToString());
                    textures.Add(texture);
                    index++;
                }
                catch (Exception)
                {
                    break;
                }
            }

            return textures.ToArray();
        }
        public static Texture2D[] LoadAsAnim(ContentManager content, string path, int frames)
        {
            List<Texture2D> textures = new List<Texture2D>();

            for (int i = 0; i < frames; i++)
            {
                Texture2D texture = content.Load<Texture2D>(path + (i+1).ToString());
                textures.Add(texture);
            }

            return textures.ToArray();
        }
        public static bool InsideScreen(Vector2 pos, StaticData data, float add = 0)
        {
            return 
                (pos.X >= 0 - add && 
                pos.X <= data.res.X + add && 
                pos.Y >= 0 - add && 
                pos.Y <= data.res.Y + add);
        }
        public static Vector2 Lerp(Vector2 start, Vector2 end, float amount)
        {
            amount = MathHelper.Clamp(amount, 0f, 1f);

            float x = start.X + (end.X - start.X) * amount;
            float y = start.Y + (end.Y - start.Y) * amount;

            return new Vector2(x, y);
        }
        public static float InverseLerpClamp(float min, float max, float value)
        {
            if (Math.Abs(max - min) < float.Epsilon)
            {
                return 0f;
            }

            return Math.Clamp((value - min) / (max - min),0f,1f);
        }
        public static float InverseLerp(float min, float max, float value)
        {
            if (Math.Abs(max - min) < float.Epsilon)
            {
                return 0f;
            }

            return (value - min) / (max - min);
        }
        public static float AngleDifference(float angle1, float angle2)
        {
            float difference = TUH.NormalizeAngle(angle2) - TUH.NormalizeAngle(angle1);

            difference %= 360f;

            if (difference <= -180f)
            {
                difference += 360f;
            }
            else if (difference > 180f)
            {
                difference -= 360f;
            }

            return difference;
        }
        public static Vector2 RandomUnitVector()
        {
            Random random = new Random();

            float angle = (float)(random.NextDouble() * 2 * Math.PI);

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector2(x, y);
        }
        public static float AngleToTarget(Vector2 position, Vector2 target)
        {
            Vector2 direction = target - position;
            if (direction == Vector2.Zero)
            {
                return 0f;
            }

            float angleFromUp_CCW_Radians = (float)Math.Atan2(direction.X, -direction.Y);

            float angleFromUp_CCW_Degrees = MathHelper.ToDegrees(angleFromUp_CCW_Radians);

            float angleFromUp_CW_Degrees = -angleFromUp_CCW_Degrees;

            angleFromUp_CW_Degrees = angleFromUp_CW_Degrees % 360f;

            return angleFromUp_CW_Degrees;
        }
        public static float Distance(Vector2 point1, Vector2 point2)
        {
            Vector2 difference = point2 - point1;
            return difference.Length();
        }
        public static float VectorToAngle(Vector2 vector)
        {
            float angleRadians = (float)Math.Atan2(vector.X, -vector.Y);
            float angleDegrees = MathHelper.ToDegrees(angleRadians);

            if (angleDegrees < 0)
                angleDegrees += 360;

            return angleDegrees;
        }   
        public static Vector2 AngleToVector(float angleDegrees)
        {
            float angleRadians = MathHelper.ToRadians(angleDegrees);

            float x = (float)Math.Sin(angleRadians);
            float y = -(float)Math.Cos(angleRadians);

            return new Vector2(x, y);
        }
        public static int CalRotationDirection(float currentAngle, float targetAngle)
        {
            float normalizedCurrent = NormalizeAngle(currentAngle);
            float normalizedTarget = NormalizeAngle(targetAngle);

            float difference = normalizedTarget - normalizedCurrent;

            if (difference > 180)
            {
                difference -= 360;
            }
            else if (difference < -180)
            {
                difference += 360;
            }

            if (Math.Abs(difference) < 0.001f)
            {
                return 0;
            }

            return difference > 0 ? 1 : -1;
        }
        public static float NormalizeAngle(float angle)
        {
            angle %= 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
    }
}
