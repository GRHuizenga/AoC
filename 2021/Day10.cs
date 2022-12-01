using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day10 : Day<double>
    {
        private char[] Opening = new char[4] { '[', '{', '(', '<' };
        private char[] Closing = new char[4] { ']', '}', ')', '>' };
        private Dictionary<char, int> ScoreLookup = new Dictionary<char, int>
        {
            { ']', 57 },
            { '}', 1197 },
            { ')', 3 },
            { '>', 25137 },
        };
        private Dictionary<char, int> ScoreLookup2 = new Dictionary<char, int>
        {
            { ']', 2 },
            { '}', 3 },
            { ')', 1 },
            { '>', 4 },
        };

        public Day10() : base(2021, "day10") { }

        public override double PartOne()
        {
            var stack = new Stack<char>();
            return InputLines.Aggregate(0, (acc, error) =>
            {
                stack.Clear();
                foreach (var ch in error)
                {
                    if (Opening.Contains(ch)) stack.Push(Closing[Array.IndexOf(Opening, ch)]);
                    else if (stack.Peek() != ch) return acc + ScoreLookup[ch];
                    else stack.Pop();
                }
                return acc;
            });
        }

        public override double PartTwo()
        {
            var closingSequences = InputLines.Aggregate(new List<Stack<char>>(), (acc, error) =>
            {
                var stack = new Stack<char>();
                foreach (var ch in error)
                {
                    if (Opening.Contains(ch)) stack.Push(Closing[Array.IndexOf(Opening, ch)]);
                    else if (stack.Peek() != ch) return acc;
                    else stack.Pop();
                }
                acc.Add(stack);
                return acc;
            });

            return closingSequences
                .Select(stack => string.Join(string.Empty, stack).Aggregate(0.0, (acc, curr) => 5 * acc + ScoreLookup2[curr]))
                .OrderBy(score => score)
                .ElementAt((int)Math.Floor((double)(closingSequences.Count() / 2)));
        }
    }
}
