using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day07 : Day<int>
    {
        private IEnumerable<int> CrabPositions;
        private int Median;

        public Day07() : base(2021, "day07")
        {
            CrabPositions = Input.First().Split(',').Select(position => int.Parse(position)).OrderBy(p => p);
            Median = CrabPositions.ToArray()[CrabPositions.Count() / 2];
        }

        public override int PartOne()
        {
            // min distance point is median in the list
            return CrabPositions.Sum(p => Math.Abs(Median - p));
        }

        public override int PartTwo()
        {
            // take median as a starting point
            var distance = CrabPositions.Sum(p => ((1 + Math.Abs(Median - p)) * Math.Abs(Median - p)) / 2);
            var minCost = int.MaxValue;
            while (minCost > distance)
            {
                Median++;
                minCost = distance;
                distance = CrabPositions.Sum(p => ((1 + Math.Abs(Median - p)) * Math.Abs(Median - p)) / 2);
            }
            return minCost;
        }
    }
}
