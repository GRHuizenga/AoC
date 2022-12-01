using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day04 : Day<int>
    {
        private IEnumerable<int> DrawnNumber;
        private IEnumerable<BingoBoard> Boards;

        public Day04() : base(2021, "day04")
        {
            DrawnNumber = InputLines.First().Split(',').Select(number => int.Parse(number));
            InputLines = InputLines.Skip(1);
            Boards = Enumerable.Range(0, InputLines.Count() / 6).Select(board =>
            {
                return new BingoBoard(string.Join(' ', InputLines.Skip(board * 6).Skip(1).Take(5)), 5);
            }).ToList();
        }

        public override int PartOne()
        {
            var score = 0;
            foreach (var draw in DrawnNumber)
            {
                var hasBingo = Boards.FirstOrDefault(board => board.MarkAndCheck(draw));
                if (hasBingo != null)
                {
                    score = hasBingo.Score;
                    break;
                }
            }
            return score;
        }

        public override int PartTwo()
        {
            return DrawnNumber.Aggregate(Boards.ToArray(), (acc, curr) =>
            {
                if (acc.Count() == 1)
                {
                    if (acc.First().Score == 0)
                    {
                        acc.First().MarkAndCheck(curr);
                    }
                    return acc;
                }
                else
                {
                    return acc.Where(board => !board.MarkAndCheck(curr)).ToArray();
                }
            }).Single().Score;
        }
    }

    public class BingoBoard
    {
        private int[] Board;
        private List<Tuple<int, int>> Hits;
        private readonly int Size;
        public int Score { get; private set; }

        public BingoBoard(string board, int size)
        {
            Board = board.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(field => int.Parse(field.Trim())).ToArray();
            Hits = new List<Tuple<int, int>>();
            Size = size;
        }

        public bool MarkAndCheck(int number)
        {
            var index = Array.IndexOf(Board, number);
            if (index > -1)
            {
                Hits.Add(Tuple.Create(index % Size, index / Size));
            }

            var hasBingo = HasBingo();
            if (hasBingo) CalculateScore(number);

            return hasBingo;
        }

        private bool HasBingo()
        {
            return Hits.GroupBy(key => key.Item1).Any(group => group.Count() == Size) ||
                Hits.GroupBy(key => key.Item2).Any(group => group.Count() == Size);
        }

        private void CalculateScore(int draw)
        {
            Score = Enumerable.Range(0, Board.Length)
                .Where(index => !Hits.Contains(Tuple.Create(index % Size, index / Size)))
                .Sum(index => Board[index]) * draw;
        }
    }
}
