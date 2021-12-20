using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day19 : Day<int>
    {
        public Day19() : base(2021, "test") { }

        public override int PartOne()
        {
            return -1;
        }

        public override int PartTwo()
        {
            return -1;
        }

        private Func<string, Vector> ToVector = (string input) =>
        {
            var coordinates = input.Split(',');
            return new Vector(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
        };
    }

    public record Scanner(int ScannerId, List<Vector> RelativeBeacons, Vector Position = default)
    {
        // when comparing scanners (scanner 0 == target, scanner X == source):
        //
        // from target.AbsoluteVectors t
        // from source.RelativeVectors s
        // offset = t - s
        // set source.Position = offset
        // check intersect between target.Absolute and source.Absolute >= 12
        //
        // do this for all orientations...or create a scanner with the same id and all beacons in 1 of the 24 orientations?
        public IEnumerable<Vector> AbsoluteBeacons
        {
            get => RelativeBeacons.Select(beacon => beacon.Add(Position));
        }
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
