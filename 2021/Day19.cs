using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2021
{
    public class Day19 : Day<int>
    {
        private IEnumerable<Scanner> Scanners;
        private IEnumerable<Scanner> LocatedScanners;

        public Day19() : base(2021, "day19")
        {
            Scanners = Parse(Input);
            LocatedScanners = Locate();
        }

        public override int PartOne()
        {
            return LocatedScanners.SelectMany(scanner => scanner.AbsoluteBeacons).Distinct().Count();
        }

        public override int PartTwo()
        {
            var positions = LocatedScanners.Select(scanner => scanner.Position);
            return positions
                .SelectMany(positionA => positions, (positionA, positionB) => positionA.Distance(positionB))
                .Max();
        }

        private IEnumerable<Scanner> Locate()
        {
            var locatedScanners = new HashSet<Scanner>();
            locatedScanners.Add(Scanners.First());
            var queue = new Queue<Scanner>(Scanners.Skip(1).ToList());

            while (queue.Any())
            {
                var target = queue.Dequeue();
                var located = false;
                foreach (var source in locatedScanners.ToList())
                {
                    var matches = source.Matches(target);
                    if (matches.Any()) {
                        located = true;
                        // find the orientation of b where the difference (a - b@orientation) is equal for all matches
                        // this is the position of the target scanner relative to the source scanner
                        var or = Enumerable.Range(0, 24).First(orientation => matches.Select(match => match.a.Subtract(match.b.AllOrientations[orientation])).Distinct().Count() == 1);
                        var scannerPos = matches.First().a.Subtract(matches.First().b.AllOrientations[or]);

                        // update the found scanner with the orientation to apply and the position of the scanner
                        locatedScanners.Add(target with { Orientation = or, Position = scannerPos });

                        // we already found a match, no need to keep searching
                        break;
                    }
                }
                // if we couldn't find a match yet, queue it again. We might find a match after more scanners are located
                if (!located) queue.Enqueue(target);
            }

            return locatedScanners;
        }

        private Func<string, Vector> ToVector = (string input) =>
        {
            var coordinates = input.Split(',');
            return new Vector(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
        };

        private IEnumerable<Scanner> Parse(IEnumerable<string> input)
        {
            int id = 0;
            List<Vector> beacons = new List<Vector>();
            var scanners = new List<Scanner>();
            foreach (var line in input.Append(string.Empty))
            {
                if (line.StartsWith("---"))
                {
                    id = int.Parse(new Regex(@"(\d+)").Match(line).Groups[0].Value);
                    beacons.Clear();
                }
                else if (string.IsNullOrEmpty(line))
                {
                    scanners.Add(new Scanner(id, beacons.ToList()));
                }
                else
                {
                    beacons.Add(ToVector(line));
                }
            }
            return scanners;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ScannerId"></param>
    /// <param name="RelativeBeacons">vectors of all beacons relative to this scanner</param>
    /// <param name="Position">position of the scanner, default is (0, 0, 0)</param>
    public record Scanner(int ScannerId, List<Vector> RelativeBeacons, int Orientation = default, Vector Position = default)
    {
        public IEnumerable<(Vector a, Vector b)> Matches(Scanner other)
        {
            return AbsoluteBeacons
                // all combinations of beacons between this scanner's absolute beacons (after applying the correct orientation and scanner position) and the other scanner's beacons
                .SelectMany(beacon => other.RelativeBeacons, (a, b) => (thisBeacon: a, otherBeacon: b))
                // where for this pair
                .Where(pair => 
                    // the intersection of equal distances of pair.thisBeacon and pair.otherBeacon's distances in it's own scanner surrounding >= 11
                    AbsoluteBeacons.Select(a => pair.thisBeacon.Distance(a))
                        .Intersect(other.RelativeBeacons.Select(b => pair.otherBeacon.Distance(b)))
                        .Count() >= 11);
        }


        public IEnumerable<Vector> AbsoluteBeacons
        {
            get => RelativeBeacons.Select(beacon => beacon.AllOrientations[Orientation].Add(Position));
        }
    }

    public record struct Vector(int X, int Y, int Z)
    {
        public Vector Subtract(Vector other) => new(X - other.X, Y - other.Y, Z - other.Z);
        public Vector Add(Vector other) => new(X + other.X, Y + other.Y, Z + other.Z);
        public int Distance(Vector other) => Math.Abs(other.X - X) + Math.Abs(other.Y - Y) + Math.Abs(other.Z - Z);

        /// <summary>
        /// Think of the scanner floating inside a cube. It can look at any of the 6 sides so we are looking for
        /// 6 possibilities here. 
        /// Flipping upside down: keep Z, negate X and Y
        /// Turning the scanner to look at the axes differently: (X, Y, Z) => (Z, X, Y)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector> Directions()
        {
            var @this = this;
            for (int i = 0; i < 3; i++)
            {
                yield return @this;
                yield return new(-@this.X, -@this.Y, @this.Z);

                @this = new(@this.Z, @this.X, @this.Y);
            }
        }

        /// <summary>
        /// Look at the rotations as the cube rolling through the water over the X-axis toward you:
        /// X stays the same, Y becomes -Z and Z becomes Y. Repeat 4 times
        /// 1,1,1 => 1,-1,1 => 1,-1,-1 => 1,1,-1
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Vector> Rotations()
        {
            var @this = this;

            for (int i = 0; i < 4; i++)
            {
                yield return @this;
                @this = new(@this.X, -@this.Z, @this.Y);
            }
        }

        /// <summary>
        /// Combining the previous 2 functions: all facing directions can be rotated 4 times: 24 orientations total.
        /// </summary>
        /// <returns></returns>
        public Vector[] AllOrientations
        {
            get => Directions().SelectMany(vector => vector.Rotations()).ToArray();
        }
    }
}
