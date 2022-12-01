using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day18 : Day<double>
    {
        public Day18() : base(2021, "day18") { }

        public override double PartOne()
        {
            var finalSum = InputLines
                .Skip(1)
                .Aggregate(ParseToNumbers(InputLines.First(), 0), (acc, rhs) =>
                {
                    var node = acc.First;
                    while (node != null)
                    {
                        node.Value = (node.Value.number, node.Value.depth + 1);
                        node = node.Next;
                    }
                    foreach (var n in ParseToNumbers(rhs)) acc.AddLast(n);
                    return Reduce(acc);
                });

            return CalculateMagnitude(finalSum);
        }

        public override double PartTwo()
        {
            return InputLines
                .Permutations()
                .SelectMany(pair => new List<(string, string)>() { pair, (pair.Item2, pair.Item1) })
                .Aggregate(0.0, (acc, pair) =>
                 {
                     var first = ParseToNumbers(pair.Item1);
                     var second = ParseToNumbers(pair.Item2);

                     foreach (var node in second) first.AddLast(node);
                     var magnitude = CalculateMagnitude(Reduce(first));
                     return acc > magnitude ? acc : magnitude;
                 });
        }

        private LinkedList<(int number, int depth)> Reduce(LinkedList<(int number, int depth)> numbers)
        {
            var node = numbers.First;
            while (node != null)
            {
                if (node.Value.depth > 4)
                {
                    var lhs = node;
                    var rhs = node.Next;
                    if (lhs.Previous != null)
                        lhs.Previous.Value = (lhs.Previous.Value.number + lhs.Value.number, lhs.Previous.Value.depth);
                    if (rhs.Next != null)
                        rhs.Next.Value = (rhs.Next.Value.number + rhs.Value.number, rhs.Next.Value.depth);
                    lhs.Value = (0, node.Value.depth - 1);
                    numbers.Remove(rhs);
                    node = numbers.First;
                }
                else if (node.Value.number > 9 && !numbers.Any(n => n.depth > 4))
                {
                    int lhs = (int)Math.Floor((double)node.Value.number / 2);
                    int rhs = (int)Math.Ceiling((double)node.Value.number / 2);
                    numbers.AddBefore(node, (lhs, node.Value.depth + 1));
                    var newNode = numbers.AddBefore(node, (rhs, node.Value.depth + 1));
                    numbers.Remove(node);
                    node = numbers.First;
                }
                else
                {
                    node = node.Next;
                }
            }
            return numbers;
        }

        private LinkedList<(int number, int depth)> ParseToNumbers(string snailfishNumber, int depth = 1)
        {
            var realNumbers = new LinkedList<(int number, int depth)>();
            for (int index = 0; index < snailfishNumber.Length; index++)
            {
                if (snailfishNumber[index] == '[') depth++;
                else if (snailfishNumber[index] == ']') depth--;
                else if (snailfishNumber[index] == ',') { /* do nothing */ }
                else realNumbers.AddLast((int.Parse(snailfishNumber[index].ToString()), depth));
            }
            return realNumbers;
        }

        private double CalculateMagnitude(LinkedList<(int number, int depth)> numbers)
        {
            var node = numbers.First;

            while (node != null)
            {
                if (node.Next != null && node.Value.depth == node.Next.Value.depth)
                {
                    node.Next.Value = (3 * node.Value.number + 2 * node.Next.Value.number, node.Value.depth - 1);
                    numbers.Remove(node);
                    node = numbers.First;
                }
                else
                {
                    node = node.Next;
                }
            }

            return numbers.First.Value.number;
        }
    }
}
