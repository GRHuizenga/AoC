using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day08 : Day<int>
    {
        public Day08() : base(2021, "day08") { }

        public override int PartOne()
        {
            var lengths = new int[4] { 2, 3, 4, 7 };
            return Input
                .Select(line => line.Split(" | ")[1].Split())
                .Aggregate(0, (acc, curr) => acc + curr.Count(output => lengths.Contains(output.Length)));
        }

        public override int PartTwo()
        {
            return Input.Aggregate(0, (acc, curr) =>
            {
                var inputsOutputs = curr.Split(" | ");
                var digits = DetermineDigits(inputsOutputs[0].Split()).Select(digit => string.Join(string.Empty, digit.OrderBy(d => d))).ToArray();
                var joined = string.Join(string.Empty, inputsOutputs[1].Split().Select(digit => Array.IndexOf(digits, string.Join(string.Empty, digit.OrderBy(d => d)))));
                return acc + int.Parse(joined);
            });
        }

        private string[] DetermineDigits(IEnumerable<string> input)
        {
            var digits = new string[10];
            digits[1] = input.Single(d => d.Length == 2);
            digits[7] = input.Single(d => d.Length == 3);
            digits[4] = input.Single(d => d.Length == 4);
            digits[8] = input.Single(d => d.Length == 7);
            digits[3] = input.Single(d => d.Length == 5 && digits[1].All(c => d.Contains(c)));
            digits[6] = input.Single(d => d.Length == 6 && !digits[1].All(c => d.Contains(c)));
            digits[5] = input.Single(d => d.Length == 5 && d.Count(c => digits[6].Contains(c)) == 5);
            digits[2] = input.Single(d => d.Length == 5 && !d.Equals(digits[3]) && !d.Equals(digits[5]));
            digits[9] = input.Single(d => d.Length == 6 && d.Count(c => !(digits[4] + digits[7]).Contains(c)) == 1);
            digits[0] = input.Single(d => d.Length == 6 && !d.Equals(digits[6]) && !d.Equals(digits[9]));
            return digits;
        }
    }
}
