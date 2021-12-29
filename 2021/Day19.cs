using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2021
{
    public class Day19 : Day<int>
    {
        private List<Scanner> Scanners;

        public Day19() : base(2021, "test")
        {
            Parse(Input);
        }

        public override int PartOne()
        {
            Locate();
            return -1;
        }

        public override int PartTwo()
        {
            return -1;
        }

        private void Locate()
        {
            var scanners = Scanners.ToList();
            var locatedScanners = new HashSet<Scanner>();

            locatedScanners.Add(scanners.First());
            scanners.Remove(scanners.First());
            var queue = new Queue<Scanner>(scanners);

            while (queue.Any())
            {
                var target = queue.Dequeue();
                foreach (var source in locatedScanners)
                {
                    var matches = source.Matches(target);
                    if (matches.Any())
                        foreach (var (sourceBeacon, targetBeacon) in matches)
                        {
                            Console.WriteLine($"{sourceBeacon.X},{sourceBeacon.Y},{sourceBeacon.Z}    =>  {targetBeacon.X},{targetBeacon.Y},{targetBeacon.Z}");
                            targetBeacon.AllOrientations().First(orientation => sourceBeacon.Subtract(orientation));
                        }
                }
            }
        }

        private Func<string, Vector> ToVector = (string input) =>
        {
            var coordinates = input.Split(',');
            return new Vector(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
        };

        private void Parse(IEnumerable<string> input)
        {
            int id = 0;
            List<Vector> beacons = new List<Vector>();
            Scanners = new List<Scanner>();
            foreach (var line in input)
            {
                if (line.StartsWith("---"))
                {
                    id = int.Parse(new Regex(@"(\d+)").Match(line).Groups[0].Value);
                    beacons.Clear();
                }
                else if (string.IsNullOrEmpty(line))
                {
                    Scanners.Add(new Scanner(id, beacons.ToList()));
                }
                else
                {
                    beacons.Add(ToVector(line));
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ScannerId"></param>
    /// <param name="RelativeBeacons">vectors of all beacons relative to this scanner</param>
    /// <param name="Position">position of the scanner, default is (0, 0, 0)</param>
    public record Scanner(int ScannerId, List<Vector> RelativeBeacons, Vector Position = default)
    {
        // when comparing scanners (scanner 0 == target, scanner X == source):
        //
        // from target.AbsoluteBeacons t
        // from source.RelativeBeacons s
        // offset = t - s
        // set source.Position = offset
        // check intersect between target.Absolute and source.Absolute >= 12
        //
        // do this for all orientations...or create a scanner with the same id and all beacons in 1 of the 24 orientations?
        public void Compare(Scanner target)
        {

        }

        public IEnumerable<(Vector a, Vector b)> Matches(Scanner other)
        {
            return RelativeBeacons
                // all combinations of beacons between this scanner's beacons and the other scanner's beacons
                .SelectMany(beacon => other.RelativeBeacons, (a, b) => (thisBeacon: a, otherBeacon: b))
                // where for this pair
                .Where(pair => 
                    // the intersection of equal distances of pair.thisBeacon and pair.otherBeacon's distances in it's own scanner surrounding >= 11
                    RelativeBeacons.Select(a => pair.thisBeacon.Distance(a))
                        .Intersect(other.RelativeBeacons.Select(b => pair.otherBeacon.Distance(b)))
                        .Count() >= 11);
        }


        public IEnumerable<Vector> AbsoluteBeacons
        {
            get => RelativeBeacons.Select(beacon => beacon.Add(Position));
        }

        /// <summary>
        /// Convenience function to get all beacons transformed/translated into each orientation
        /// </summary>
        //public IEnumerable<(int orientation, IEnumerable<Vector> beacons)> BeaconsPerOrientation
        //{
        //    get => 
        //}
    }

    public record Vector(int X, int Y, int Z)
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
        public IEnumerable<Vector> AllOrientations()
        {
            return Directions().SelectMany(vector => vector.Rotations());
        }
    }
}
