using Core;
using System.Collections.Generic;
using System.Linq;

namespace _2016.Days
{
    public class Day6: Day
    {
        private IEnumerable<IOrderedEnumerable<KeyValuePair<char, int>>> Count;

        public Day6() : base(@"Day6.txt")
        {
            Count = Lines
                .Select(line => line.ToCharArray().AsEnumerable())
                .Transpose()
                .Select(column => column.ToHashSet().AsEnumerable().ToDictionary(c => c, c => column.Count(chr => chr == c)).OrderByDescending(c => c.Value));
        }

        public string PartOne()
        {
            return string.Join(string.Empty, Count.Select(c => c.First().Key));
        }

        public string PartTwo()
        {
            return string.Join(string.Empty, Count.Select(c => c.Last().Key));
        }
    }
}
