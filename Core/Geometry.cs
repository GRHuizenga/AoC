using System;
using System.Collections.Generic;

namespace _2021
{
    public static class Geometry
    {
        /// <summary>
        /// Bresenham's Line Algorithm for finding all points on a line.
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <returns></returns>
        public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            var dx = Math.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = -Math.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;
            var error = dx + dy;

            while (true)
            {
                yield return new Point(x0, y0);

                if (x0 == x1 && y0 == y1) yield break;
                var e2 = 2 * error;
                if (e2 >= dy)
                {
                    error += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {
                    error += dx;
                    y0 += sy;
                }
            }
        }
    }

    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
