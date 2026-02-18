using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    class PolygonHitbox : Hitbox
    {
        public List<Vector2> points { get; set; }

        public override PolygonHitbox Clone()
        {
            PolygonHitbox clone = new PolygonHitbox(points);

            clone.parent = parent;
            clone.pos = pos;
            clone.depth = depth;

            return clone;
        }

        public PolygonHitbox(List<Vector2> points)
        {
            this.points = points ?? throw new ArgumentNullException(nameof(points));
            if (points.Count < 3)
                throw new ArgumentException("A polygon must have at least 3 points.", nameof(points));
        }

        public override bool Check(Vector2 GlobalPoint)
        {
            Vector2 localPoint = GlobalPoint - GetGlobalPos();
            return IsPointInPolygon(localPoint, points);
        }

        public override bool CheckLine(Vector2 pos1, Vector2 pos2)
        {
            var globalPoints = GetGlobalPoints();

            for (int i = 0; i < globalPoints.Count; i++)
            {
                int j = (i + 1) % globalPoints.Count;
                if (LineLineIntersection(pos1, pos2, globalPoints[i], globalPoints[j]))
                {
                    return true;
                }
            }

            return IsPointInPolygon(pos1 - GetGlobalPos(), points) ||
                   IsPointInPolygon(pos2 - GetGlobalPos(), points);
        }

        private static bool LineLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            float denom = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);
            if (Math.Abs(denom) < float.Epsilon) return false;

            float t = ((p1.X - p3.X) * (p3.Y - p4.Y) - (p1.Y - p3.Y) * (p3.X - p4.X)) / denom;
            float u = -((p1.X - p2.X) * (p1.Y - p3.Y) - (p1.Y - p2.Y) * (p1.X - p3.X)) / denom;

            return t >= 0 && t <= 1 && u >= 0 && u <= 1;
        }

        public override float DistanceToCircleBoundary(Vector2 point)
        {
            var globalPoints = GetGlobalPoints();
            float minDistanceSquared = float.MaxValue;

            for (int i = 0; i < globalPoints.Count; i++)
            {
                int j = (i + 1) % globalPoints.Count;
                Vector2 closestPointOnEdge = CollisionSolver.GetClosestPointOnSegment(point, globalPoints[i], globalPoints[j]);
                float distanceSquared = Vector2.DistanceSquared(point, closestPointOnEdge);

                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                }
            }

            return (float)Math.Sqrt(minDistanceSquared);
        }

        public override Vector2 GetCenter()
        {
            if (points == null || points.Count == 0)
                throw new ArgumentException("Polygon points cannot be null or empty.", nameof(points));

            if (points.Count == 1)
                return points[0];

            if (points.Count == 2)
                return (points[0] + points[1]) * 0.5f;

            Vector2 centroid = Vector2.Zero;
            float signedArea = 0f;
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                float f = (points[i].X * points[j].Y - points[j].X * points[i].Y);
                signedArea += f;
                centroid.X += (points[i].X + points[j].X) * f;
                centroid.Y += (points[i].Y + points[j].Y) * f;
            }

            signedArea *= 0.5f;

            if (Math.Abs(signedArea) < float.Epsilon)
                return GetSimpleAverage(points);

            centroid.X /= (6.0f * signedArea);
            centroid.Y /= (6.0f * signedArea);

            return centroid;
        }

        private static Vector2 GetSimpleAverage(List<Vector2> points)
        {
            Vector2 sum = Vector2.Zero;
            foreach (var point in points)
            {
                sum += point;
            }
            return sum / points.Count;
        }

        public static bool IsPointInPolygon(Vector2 point, List<Vector2> polygonPoints)
        {
            bool inside = false;
            int j = polygonPoints.Count - 1;

            for (int i = 0; i < polygonPoints.Count; i++)
            {
                Vector2 pi = polygonPoints[i];
                Vector2 pj = polygonPoints[j];

                if (((pi.Y > point.Y) != (pj.Y > point.Y)) &&
                    (point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y) + pi.X))
                {
                    inside = !inside;
                }
                j = i;
            }

            return inside;
        }
        public List<Vector2> GetGlobalPoints()
        {
            Vector2 globalPos = GetGlobalPos();
            return points.Select(p => globalPos + p).ToList();
        }
    }
}