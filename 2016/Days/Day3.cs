using Core;
using System;
using System.Linq;

namespace _2016.Days
{
    public class Day3 : Day
    {
        public Day3() : base(@"Day3.txt")
        {
        }

        public int PartOne()
        {
            var triangles = Lines.Select(line => line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)).Where(ValidTriangle);
            return triangles.Count();
        }

        public int PartTwo()
        {
            var transposedInput = Lines.Select(line => line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)).Select(x => x.AsEnumerable()).Transpose();

            var result = 0;
            foreach (var row in transposedInput)
            {
                result += row.Group(3).Select(x => x.ToArray()).Where(ValidTriangle).Count();
            }

            return result;
        }

        private Func<string[], bool> ValidTriangle = (string[] sideLengths) =>
        {
            var sides = sideLengths.Select(side => int.Parse(side)).ToArray();
            return sides[0] + sides[1] > sides[2] && sides[1] + sides[2] > sides[0] && sides[0] + sides[2] > sides[1];
        };
    }
}
