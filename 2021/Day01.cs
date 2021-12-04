using Core;
using System;
using System.Linq;

namespace _2021
{
    public class Day01 : Day<int>
    {
        public Day01(): base(2021, "day01") { }

        public override int PartOne()
        {
            var tuples = Input.Zip(Input.Skip(1), (left, right) => Tuple.Create(int.Parse(left), int.Parse(right)));
            return tuples.Count(tuple => tuple.Item2 > tuple.Item1);
        }

        public override int PartTwo()
        {
            var sums = Input.Select(measurement => int.Parse(measurement)).SlidingWindow(3).Select(triplet => triplet.Sum());
            return sums.Zip(sums.Skip(1), (left, right) => Tuple.Create(left, right)).Count(tuple => tuple.Item2 > tuple.Item1);
        }
    }
}
