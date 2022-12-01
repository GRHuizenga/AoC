using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2021
{
    public class Day22 : Day<double>
    {
        public Day22() : base(2021, "day22") { }

        public override double PartOne()
        {
            return Reboot().Sum(cuboid => cuboid.Volume);
        }

        public override double PartTwo()
        {
            return Reboot(true).Sum(cuboid => (double)cuboid.Volume);
        }

        private IEnumerable<Cuboid> Reboot(bool includeAll = false)
        {
            var cuboids = new List<Cuboid>();
            var cuboidsAfterSplit = new List<Cuboid>();
            foreach (var instruction in InputLines)
            {
                var on = instruction.StartsWith("on");
                var matches = Regex.Matches(instruction, @"(-?\d+)").Select(match => int.Parse(match.Value)).ToArray();
                var newCuboid = new Cuboid(new Vector3(matches[0], matches[2], matches[4]), new Vector3(matches[1], matches[3], matches[5]));

                if (!includeAll && (matches[0] < -50 || matches[1] > 50 || matches[2] < -50 || matches[3] > 50 || matches[4] < -50 || matches[5] > 50))
                    continue;

                foreach (var cuboid in cuboids)
                    cuboidsAfterSplit.AddRange(cuboid.SplitAroundIntersection(newCuboid).ToList());
                
                if (on) cuboidsAfterSplit.Add(newCuboid);

                (cuboids, cuboidsAfterSplit) = (cuboidsAfterSplit, cuboids);
                cuboidsAfterSplit.Clear();
            }
            return cuboids;
        }
    }
}
