using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day09 : Day<int>
    {
        private IEnumerable<IEnumerable<int>> HeightMapPadded;

        public Day09() : base(2021, "test")
        {
            HeightMapPadded = Input
                .Select(heights => heights.Split().Select(h => int.Parse(h)).Prepend(10).Append(10))
                .Transpose()
                .Select(heights => heights.Prepend(10).Append(10));
        }

        public override int PartOne()
        {
            return -1;
        }

        public override int PartTwo()
        {
            return -1;
        }
    }
}
