using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public static class RectPanelSolver
    {
        public static List<Rect> GetRectMap(Rect main, List<Rect> closed)
        {
            var xCoords = new SortedSet<float> { main.position.X, main.position.X + main.size.X };
            var yCoords = new SortedSet<float> { main.position.Y, main.position.Y + main.size.Y };

            foreach (var rect in closed)
            {
                if (!rect.CollideWith(main)) continue;

                xCoords.Add(MathHelper.Max(rect.position.X, main.position.X));
                xCoords.Add(MathHelper.Min(rect.position.X + rect.size.X, main.position.X + main.size.X));
                yCoords.Add(MathHelper.Max(rect.position.Y, main.position.Y));
                yCoords.Add(MathHelper.Min(rect.position.Y + rect.size.Y, main.position.Y + main.size.Y));
            }

            var xList = xCoords.ToList();
            var yList = yCoords.ToList();

            var freeCells = new List<Rect>();
            for (int i = 0; i < xList.Count - 1; i++)
            {
                for (int j = 0; j < yList.Count - 1; j++)
                {
                    var cell = new Rect(
                        new Vector2(xList[i], yList[j]),
                        new Vector2(xList[i + 1] - xList[i], yList[j + 1] - yList[j])
                    );

                    bool isBlocked = false;
                    foreach (var block in closed)
                    {
                        if (cell.CollideWith(block))
                        {
                            if (block.CheckPoint(cell.Center))
                            {
                                isBlocked = true;
                                break;
                            }
                        }
                    }

                    if (!isBlocked)
                        freeCells.Add(cell);
                }
            }

            return freeCells;
        }

        private static List<Rect> MergeHorizontal(List<Rect> rects)
        {
            if (rects.Count == 0) return rects;

            var result = new List<Rect>();
            var grouped = rects
                .OrderBy(r => r.position.Y)
                .ThenBy(r => r.position.X)
                .GroupBy(r => new { Y = r.position.Y, H = r.size.Y });

            foreach (var group in grouped)
            {
                var row = group.OrderBy(r => r.position.X).ToList();
                Rect current = row[0];

                for (int i = 1; i < row.Count; i++)
                {
                    var next = row[i];
                    if (Math.Abs(current.position.X + current.size.X - next.position.X) < 0.001f)
                    {
                        current = new Rect(
                            current.position,
                            new Vector2(current.size.X + next.size.X, current.size.Y)
                        );
                    }
                    else
                    {
                        result.Add(current);
                        current = next;
                    }
                }
                result.Add(current);
            }

            return result;
        }

        private static List<Rect> MergeVertical(List<Rect> rects)
        {
            if (rects.Count == 0) return rects;

            var result = new List<Rect>();

            var grouped = rects
                .OrderBy(r => r.position.X)
                .ThenBy(r => r.position.Y)
                .GroupBy(r => new { X = r.position.X, W = r.size.X });

            foreach (var group in grouped)
            {
                var column = group.OrderBy(r => r.position.Y).ToList();
                Rect current = column[0];

                for (int i = 1; i < column.Count; i++)
                {
                    var next = column[i];
                    if (Math.Abs(current.position.Y + current.size.Y - next.position.Y) < 0.001f)
                    {
                        current = new Rect(
                            current.position,
                            new Vector2(current.size.X, current.size.Y + next.size.Y)
                        );
                    }
                    else
                    {
                        result.Add(current);
                        current = next;
                    }
                }
                result.Add(current);
            }

            return result;
        }

        private static List<Rect> MergeBoth(List<Rect> rects)
        {
            if (rects.Count == 0) return rects;

            var mergedH = MergeHorizontal(rects);
            var mergedV = MergeVertical(mergedH);
            return mergedV;
        }
    }
}