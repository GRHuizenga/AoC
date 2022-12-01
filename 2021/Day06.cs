using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day06 : Day<double>
    {
        private List<int> InitialState;

        public Day06() : base(2021, "day06")
        {
            InitialState = InputLines.First().Split(',').Select(s => int.Parse(s)).ToList();
        }

        public override double PartOne()
        {
            return Enumerable.Range(0, 80).Aggregate(InitialState, (acc, curr) =>
            {
                acc = acc.Select(s => --s).ToList();
                var c = acc.Where(s => s < 0).Count();
                acc = acc.Select(s => s < 0 ? 6 : s).ToList();
                for (int i = 0; i < c; i++)
                {
                    acc.Add(8);
                }
                return acc;
            }).Count();
        }

        public override double PartTwo()
        {
            var state = new double[9];
            foreach (var index in Enumerable.Range(0, 9))
            {
                state[index] = InitialState.Count(s => s == index);
            }
            
            return Enumerable.Range(0, 256).Aggregate(state, (acc, curr) =>
            {
                var zeros = acc[0];
                var newState = new double[9];
                Array.Copy(acc, 1, newState, 0, acc.Length - 1);
                newState[6] = newState[6] + zeros;
                newState[8] = zeros;
                return newState;
            }).Sum();
        }
    }
}
