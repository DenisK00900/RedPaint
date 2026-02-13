using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    static class CollisionSolver
    {
        public static bool Check(Hitbox hb1, Hitbox hb2)
        {
            if (hb1 is CircleHitbox c1 && hb2 is CircleHitbox c2)
            {
                return Vector2.DistanceSquared(hb1.GetGlobalPos(), hb2.GetGlobalPos()) <= (c1.r + c2.r) * (c1.r + c2.r);
            }

            if (hb1 is PolygonHitbox p1 && hb2 is PolygonHitbox p2)
            {
                return CheckPolygonPolygon(p1, p2);
            }

            if (hb1 is PolygonHitbox polygonHb && hb2 is CircleHitbox circleHb)
            {
                return CheckPolygonCircle(polygonHb, circleHb);
            }
            if (hb1 is CircleHitbox circleHb2 && hb2 is PolygonHitbox polygonHb2)
            {
                return CheckPolygonCircle(polygonHb2, circleHb2);
            }

            return false;
        }

        private static bool CheckPolygonPolygon(PolygonHitbox p1, PolygonHitbox p2)
        {
            var points1 = p1.GetGlobalPoints();
            var points2 = p2.GetGlobalPoints();

            for (int i = 0; i < points1.Count; i++)
            {
                var j = (i + 1) % points1.Count;
                var edge = points1[j] - points1[i];
                var axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                var (min1, max1) = ProjectPolygon(axis, points1);
                var (min2, max2) = ProjectPolygon(axis, points2);

                if (max1 < min2 || max2 < min1)
                {
                    return false;
                }
            }

            for (int i = 0; i < points2.Count; i++)
            {
                var j = (i + 1) % points2.Count;
                var edge = points2[j] - points2[i];
                var axis = new Vector2(-edge.Y, edge.X);
                axis = Vector2.Normalize(axis);

                var (min1, max1) = ProjectPolygon(axis, points1);
                var (min2, max2) = ProjectPolygon(axis, points2);

                if (max1 < min2 || max2 < min1)
                {
                    return false;
                }
            }

            return true;
        }
        
        private static (float min, float max) ProjectPolygon(Vector2 axis, List<Vector2> points)
        {
            float min = Vector2.Dot(axis, points[0]);
            float max = min;
            for (int i = 1; i < points.Count; i++)
            {
                float projection = Vector2.Dot(axis, points[i]);
                if (projection < min) min = projection;
                if (projection > max) max = projection;
            }
            return (min, max);
        }

        private static bool CheckPolygonCircle(PolygonHitbox polygonHb, CircleHitbox circleHb)
        {
            var polygonPoints = polygonHb.GetGlobalPoints();
            var circleCenter = circleHb.GetGlobalPos();
            var radius = circleHb.r;

            if (PolygonHitbox.IsPointInPolygon(circleCenter, polygonPoints))
            {
                return true;
            }

            for (int i = 0; i < polygonPoints.Count; i++)
            {
                var j = (i + 1) % polygonPoints.Count;
                var closestPointOnEdge = GetClosestPointOnSegment(circleCenter, polygonPoints[i], polygonPoints[j]);
                var distanceSquared = Vector2.DistanceSquared(circleCenter, closestPointOnEdge);
                if (distanceSquared <= radius * radius)
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector2 GetClosestPointOnSegment(Vector2 point, Vector2 segA, Vector2 segB)
        {
            var segment = segB - segA;
            var lengthSquared = segment.LengthSquared();
            if (lengthSquared == 0) return segA;

            var t = Vector2.Dot(point - segA, segment) / lengthSquared;
            t = Math.Clamp(t, 0f, 1f);
            return segA + t * segment;
        }
    }
}