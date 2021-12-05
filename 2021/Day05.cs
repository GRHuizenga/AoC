using Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2021
{
    public class Day05 : Day<int>
    {
        private Regex AllDigits = new Regex(@"(\d+)");

        public Day05() : base(2021, "day05") { }

        public override int PartOne()
        {
            var points = GetPoints();
            return points.ToHashSet(new PointEqualityComparer())
                .ToDictionary(p => p, p => points.Count(point => point.X == p.X && point.Y == p.Y))
                .Count(point => point.Value >= 2);
        }

        public override int PartTwo()
        {
            var points = GetPoints(true);
            return points.ToHashSet(new PointEqualityComparer())
                .ToDictionary(p => p, p => points.Count(point => point.X == p.X && point.Y == p.Y))
                .Count(point => point.Value >= 2);
        }

        private IEnumerable<Point> GetPoints(bool includeDiagonal = false)
        {
            return Input
                .Select(line => AllDigits.Matches(line).Select(match => int.Parse(match.Value)).ToArray())
                .Where(points => includeDiagonal || (points[0] == points[2] || points[1] == points[3]))
                .SelectMany(points => Geometry.GetPointsOnLine(points[0], points[1], points[2], points[3]));
        }
    }

    public class PointEqualityComparer: IEqualityComparer<Point>
    {
        public bool Equals(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public int GetHashCode([DisallowNull] Point p)
        {
            int hCode = p.X ^ p.Y;
            return hCode.GetHashCode();
        }
    }
}
