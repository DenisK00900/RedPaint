using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    internal class CircleHitbox : Hitbox
    {
        public float r {  get; set; }

        public override bool Check(Vector2 globalpoint)
        {
            return (globalpoint - GetGlobalPos()).LengthSquared() <= r * r;
        }

        public override float DistanceToCircleBoundary(Vector2 point)
        {
            Vector2 center = GetGlobalPos();
            float distanceToCenter = Vector2.Distance(point, center);
            return distanceToCenter - r;
        }

        public override bool CheckLine(Vector2 pos1, Vector2 pos2)
        {
            Vector2 d = pos2 - pos1;
            Vector2 f = pos1 - GetGlobalPos();

            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - r * r;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }

            discriminant = (float)Math.Sqrt(discriminant);
            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);

            return (t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1);
        }

        public override Vector2 GetCenter()
        {
            return GetGlobalPos();
        }

        public CircleHitbox(float r, AbstrEntity parent = null)
        {
            this.r = r;
            this.pos = Vector2.Zero;
            this.parent = parent;
        }

        public CircleHitbox(float r, Vector2 pos, AbstrEntity parent = null)
        {
            this.r = r;
            this.pos = pos;
            this.parent = parent;
        }
    }
}
