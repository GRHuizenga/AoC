using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day03 : Day<int>
    {
        public Day03() : base(2021, "day03") { }

        public override int PartOne()
        {
            var rates = InputLines
                .Select(bin => bin.ToCharArray()).Transpose().Select(bin =>
                {
                    var ones = bin.Count(b => b == '1');
                    var zeros = bin.Count() - ones;
                    return new
                    {
                        GammaRate = ones > zeros ? "1" : "0",
                        EpsilonRate = ones > zeros ? "0" : "1",
                    };
                })
                .Aggregate(new Rates(), (acc, rates) =>
                {
                    acc.Gamma += rates.GammaRate;
                    acc.Epsilon += rates.EpsilonRate;
                    return acc;
                });
                
            return Convert.ToInt32(rates.Gamma, 2) * Convert.ToInt32(rates.Epsilon, 2);
        }

        public override int PartTwo()
        {
            var count = InputLines.First().Length;
            var lsr = new LifeSupportRating();
            lsr.OxygenGeneratorRating = InputLines.Select(line => line);
            lsr.Co2ScrubberRating = InputLines.Select(line => line);
            var rates = Enumerable.Range(0, count).Aggregate(lsr, (acc, index) =>
            {
                if (acc.OxygenGeneratorRating.Count() > 1)
                {
                    var ones = acc.OxygenGeneratorRating.Select(b => b.ToCharArray()).Transpose().Skip(index).First().Count(b => b == '1');
                    var zeros = acc.OxygenGeneratorRating.Count() - ones;
                    acc.OxygenGeneratorRating = acc.OxygenGeneratorRating.Where(line => line.ToArray()[index] == (ones >= zeros ? '1' : '0'));
                }

                if (acc.Co2ScrubberRating.Count() > 1)
                {
                    var ones = acc.Co2ScrubberRating.Select(b => b.ToCharArray()).Transpose().Skip(index).First().Count(b => b == '1');
                    var zeros = acc.Co2ScrubberRating.Count() - ones;
                    acc.Co2ScrubberRating = acc.Co2ScrubberRating.Where(line => line.ToArray()[index] == (ones >= zeros ? '0' : '1'));
                }

                return acc;
            });
            return Convert.ToInt32(rates.OxygenGeneratorRating.First(), 2) * Convert.ToInt32(rates.Co2ScrubberRating.First(), 2);
        }
    }

    public class Rates
    {
        public string Gamma { get; set; }
        public string Epsilon { get; set; }

        public Rates()
        {
            Gamma = string.Empty;
            Epsilon = string.Empty;
        }
    }

    public class LifeSupportRating
    {
        public IEnumerable<string> OxygenGeneratorRating { get; set; }
        public IEnumerable<string> Co2ScrubberRating { get; set; }
    }
}
