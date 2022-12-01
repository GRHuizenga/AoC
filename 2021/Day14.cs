using Core;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day14 : Day<double>
    {
        private string PolymerTemplate;
        private Dictionary<string, string> InsertionRules;

        public Day14() : base(2021, "day14")
        {
            PolymerTemplate = InputLines.First();
            InsertionRules = InputLines
                .Skip(2)
                .Select(rule => rule.Split(" -> "))
                .ToDictionary(rule => rule[0], rule => rule[1]);
        }

        public override double PartOne()
        {
            return Count(10);
        }

        public override double PartTwo()
        {
            return Count(40);
        }

        private double Count(int steps)
        {
            Dictionary<string, double> dict = InsertionRules.Select(pair => pair.Key).ToDictionary(pair => pair, pair => 0.0);
            Dictionary<char, double> counts = string.Join(string.Empty, InsertionRules.Select(rule => rule.Key))
                .ToHashSet()
                .ToDictionary(c => c, c => PolymerTemplate.Contains(c) ? PolymerTemplate.Count(d => d == c) : 0.0);

            foreach (var pair in PolymerTemplate.SlidingWindow(2)) ++dict[string.Join(string.Empty, pair)];
            foreach (var step in Enumerable.Range(0, steps))
            {
                foreach (var pair in dict.Where(pair => pair.Value > 0).ToList())
                {
                    // split pair and update count of split pair and new pairs
                    dict[pair.Key] -= pair.Value;
                    (string lhs, string rhs) = (pair.Key[0] + InsertionRules[pair.Key], InsertionRules[pair.Key] + pair.Key[1]);
                    dict[lhs] += pair.Value;
                    dict[rhs] += pair.Value;

                    // add count of the inserted letter
                    counts[InsertionRules[pair.Key][0]] += pair.Value;
                }
            }

            var ordered = counts.OrderByDescending(c => c.Value);

            return ordered.First().Value - ordered.Last().Value;
        }
    }
}
