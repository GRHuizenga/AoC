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

    public record struct Vector3(int X, int Y, int Z)
    {
        public Vector3 Subtract(Vector3 other) => new(X - other.X, Y - other.Y, Z - other.Z);
        public Vector3 Add(Vector3 other) => new(X + other.X, Y + other.Y, Z + other.Z);
        public int Distance(Vector3 other) => Math.Abs(other.X - X) + Math.Abs(other.Y - Y) + Math.Abs(other.Z - Z);
    }

    public record struct Cuboid(Vector3 From, Vector3 To)
    {
        public Cuboid Intersection(Cuboid other)
        {
            var from = new Vector3(Math.Max(From.X, other.From.X), Math.Max(From.Y, other.From.Y), Math.Max(From.Z, other.From.Z));
            var to = new Vector3(Math.Min(To.X, other.To.X), Math.Min(To.Y, other.To.Y), Math.Min(To.Z, other.To.Z));
            return new Cuboid(from, to);
        }

        public IEnumerable<Cuboid> SplitAroundIntersection(Cuboid other)
        {
            var intersection = Intersection(other);
            if (!intersection.Valid) yield return this;
            else
            {
                if (intersection.From.X > From.X)
                    yield return new Cuboid(new Vector3(From.X, From.Y, From.Z), new Vector3(intersection.From.X - 1, To.Y, To.Z));
                if (intersection.To.X < To.X)
                    yield return new Cuboid(new Vector3(intersection.To.X + 1, From.Y, From.Z), new Vector3(To.X, To.Y, To.Z));
                if (intersection.From.Y > From.Y)
                    yield return new Cuboid(new Vector3(intersection.From.X, From.Y, From.Z), new Vector3(intersection.To.X, intersection.From.Y - 1, To.Z));
                if (intersection.To.Y < To.Y)
                    yield return new Cuboid(new Vector3(intersection.From.X, intersection.To.Y + 1, From.Z), new Vector3(intersection.To.X, To.Y, To.Z));
                if (intersection.From.Z > From.Z)
                    yield return new Cuboid(new Vector3(intersection.From.X, intersection.From.Y, From.Z), new Vector3(intersection.To.X, intersection.To.Y, intersection.From.Z - 1));
                if (intersection.To.Z < To.Z)
                    yield return new Cuboid(new Vector3(intersection.From.X, intersection.From.Y, intersection.To.Z + 1), new Vector3(intersection.To.X, intersection.To.Y, To.Z));
            }
        }

        public bool Valid
        {
            get => From.X <= To.X && From.Y <= To.Y && From.Z <= To.Z;
        }

        public double Volume
        {
            get => (double)(To.X - From.X + 1) * (double)(To.Y - From.Y + 1) * (double)(To.Z - From.Z + 1);
        }
    }
}
