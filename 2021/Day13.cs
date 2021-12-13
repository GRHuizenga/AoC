using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day13 : Day<int>
    {
        private HashSet<(int x, int y)> Dots;
        private IEnumerable<(char direction, int line)> FoldInstructions;

        public Day13() : base(2021, "day13") { }

        public override int PartOne()
        {
            ParseInput();
            Fold(FoldInstructions.First());
            return Dots.Count();
        }

        public override int PartTwo()
        {
            ParseInput();
            foreach (var foldInstruction in FoldInstructions) Fold(foldInstruction);

            var grid = Enumerable
                .Range(0, Dots.Max(dot => dot.y) + 1)
                .Select(y => string.Join(string.Empty, Enumerable.Range(0, Dots.Max(dot => dot.x) + 1).Reverse().Select(x => Dots.Contains((x, y)) ? '#' : ' ')).Trim());

            foreach (var row in grid) Console.WriteLine(row);

            return -1;
        }

        private void Fold((char direction, int line) foldInstruction)
        {
            if (foldInstruction.direction == 'x') FoldX(foldInstruction.line);
            else FoldY(foldInstruction.line);
        }

        private void FoldX(int line)
        {
            foreach (var dot in Dots.Where(d => d.x < line).ToList())
            {
                Dots.Add((line + (line - dot.x), dot.y));
            }
            Dots.RemoveWhere(dot => dot.x <= line);
            Dots = Dots.Select(dot => (dot.x - (line + 1), dot.y)).ToHashSet();
        }

        private void FoldY(int line)
        {
            foreach (var dot in Dots.Where(d => d.y > line).ToList())
            {
                Dots.Add((dot.x, line - (dot.y - line)));
                Dots.Remove(dot);
            }
        }

        private void ParseInput()
        {
            Dots = Input
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(dot => (int.Parse(dot.Split(',')[0]), int.Parse(dot.Split(',')[1])))
                .ToHashSet();

            FoldInstructions = Input
                .Skip(Dots.Count() + 1)
                .Select(instruction =>
                {
                    var inst = instruction.Replace("fold along ", string.Empty).Split('=');
                    return (inst[0].ElementAt(0), int.Parse(inst[1]));
                });
        }
    }
}
