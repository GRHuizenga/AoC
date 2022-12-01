using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day25 : Day<int>
    {
        private char[][] Grid;
        private int Width;
        private int Height;
        private IEnumerable<(int y, int x)> Coordinates;

        public Day25() : base(2021, "day25")
        {
            Grid = InputLines.Select(line => line.ToCharArray()).ToArray();
            Width = Grid[0].Length;
            Height = Grid.Length;
            Coordinates = Enumerable.Range(0, Height).SelectMany(y => Enumerable.Range(0, Width), (y, x) => (y, x));
        }

        public override int PartOne()
        {
            var moves = 0;
            var advanced = int.MaxValue;
            while (advanced > 0)
            {
                advanced = MoveEast() + MoveSouth();
                moves++;
            }
            return moves;
        }

        public override int PartTwo()
        {
            return -1;
        }

        private int MoveEast()
        {
            var advance = new List<(int y, int x, int xNext)>();
            foreach (var (y, x) in Coordinates)
            {
                var xNext = (x + 1) % Width;
                if (Grid[y][x] == '>' && Grid[y][xNext] == '.') advance.Add((y, x, xNext));
            }

            foreach (var (y, x, xNext) in advance)
            {
                Grid[y][x] = '.';
                Grid[y][xNext] = '>';
            }

            return advance.Count;
        }

        private int MoveSouth()
        {
            var advance = new List<(int y, int x, int yNext)>();
            foreach(var (y, x) in Coordinates)
            {
                var yNext = (y + 1) % Height;
                if (Grid[y][x] == 'v' && Grid[yNext][x] == '.') advance.Add((y, x, yNext));
            }

            foreach (var (y, x, yNext) in advance)
            {
                Grid[y][x] = '.';
                Grid[yNext][x] = 'v';
            }

            return advance.Count;
        }
    }
}
