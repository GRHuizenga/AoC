using System;

namespace _2016.Days
{
    public class Day2: Day
    {
        private int[,] KeyPad = new int[3, 3]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 },
        };

        private char[,] DiamondKeyPad = new char[5, 5]
        {
            { '-', '-', '1', '-', '-' },
            { '-', '2', '3', '4', '-' },
            { '5', '6', '7', '8', '9' },
            { '-', 'A', 'B', 'C', '-' },
            { '-', '-', 'D', '-', '-' },
        };

        public Day2() : base(@"Day2.txt") { }

        public string PartOne()
        {
            var point = new Point(1, 1);
            var result = string.Empty;
            foreach (var line in Lines)
            {
                foreach (var instruction in line)
                {
                    point = instruction switch
                    {
                        'L' => new Point(Math.Max(0, point.X - 1), point.Y),
                        'R' => new Point(Math.Min(2, point.X + 1), point.Y),
                        'U' => new Point(point.X, Math.Max(0, point.Y - 1)),
                        'D' => new Point(point.X, Math.Min(2, point.Y + 1)),
                        _ => throw new ArgumentOutOfRangeException(nameof(instruction), $"Unexpected character '{instruction}' in input string"),
                    };
                }
                result += KeyPad[point.Y, point.X];
            }
            return result;
        }

        public string PartTwo()
        {
            var point = new DiamondPoint(2, 0);
            var result = string.Empty;
            foreach (var line in Lines)
            {
                foreach (var instruction in line)
                {
                    switch (instruction)
                    {
                        case 'L': point.SetX(Math.Max(0, point.X - 1), DiamondKeyPad[point.Y, Math.Max(0, point.X - 1)]); break;
                        case 'R': point.SetX(Math.Min(4, point.X + 1), DiamondKeyPad[point.Y, Math.Min(4, point.X + 1)]); break;
                        case 'U': point.SetY(Math.Max(0, point.Y - 1), DiamondKeyPad[Math.Max(0, point.Y - 1), point.X]); break;
                        case 'D': point.SetY(Math.Min(4, point.Y + 1), DiamondKeyPad[Math.Min(4, point.Y + 1), point.X]); break;
                        default: throw new ArgumentOutOfRangeException(nameof(instruction), $"Unexpected character '{instruction}' in input string");
                    };
                }
                result += DiamondKeyPad[point.Y, point.X];
            }
            return result;
        }
    }

    public class Point
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class DiamondPoint: Point
    {
        public DiamondPoint(int x, int y): base(x, y) { }

        public void SetX(int x, char newLocation)
        {
            if (!IsEdge(newLocation)) X = x;
        }

        public void SetY(int y, char newLocation)
        {
            if (!IsEdge(newLocation)) Y = y;
        }

        public bool IsEdge(char c) => c == '-';
    }
}
