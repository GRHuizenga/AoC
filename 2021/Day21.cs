using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day21 : Day<double>
    {
        private (int square, int score) PlayerOne;
        private (int square, int score) PlayerTwo;

        public Day21() : base(2021, "day21") { }

        public override double PartOne()
        {
            ParseInput();
            var turn = 1;
            while (PlayerOne.score < 1000 && PlayerTwo.score < 1000)
            {
                var start = (((turn - 1) * 3) % 100) + 1;
                var first = ((start - 1) % 100) + 1;
                var second = (start % 100) + 1;
                var third = ((start + 1) % 100) + 1;
                var roll = first + second + third;
                if (turn % 2 == 1)
                {
                    PlayerOne.square = ((PlayerOne.square + roll - 1) % 10) + 1;
                    PlayerOne.score += PlayerOne.square;
                }
                else
                {
                    PlayerTwo.square = ((PlayerTwo.square + roll - 1) % 10) + 1;
                    PlayerTwo.score += PlayerTwo.square;
                }
                turn++;
            }

            var losingPlayer = PlayerOne.score > PlayerTwo.score ? PlayerTwo : PlayerOne;
            return losingPlayer.score * (turn - 1) * 3;
        }

        public override double PartTwo()
        {
            ParseInput();
            var wins = PlayTurn(PlayerOne, PlayerTwo, 1);
            return Math.Max(wins.player1Wins, wins.player2Wins);
        }

        private Dictionary<(int square, int score), int> MemoizationCache = new();
        private static IEnumerable<int> DiracDie = Enumerable.Range(1, 3);
        private static IEnumerable<int> DiracScores = DiracDie
            .SelectMany(die2 => DiracDie, (die1, die2) => die1 + die2)
            .SelectMany(die3 => DiracDie, (die12, die3) => die12 + die3);
        private Dictionary<int, int> DiracScoreFrequencies = DiracScores
            .ToHashSet()
            .ToDictionary(score => score, score => DiracScores.Count(s => score == s));

        private (double player1Wins, double player2Wins) PlayTurn((int square, int score) player1, (int square, int score) player2, int turn)
        {
            (double p1Wins, double p2Wins) wins = (0, 0);
            foreach (var rolled in DiracScoreFrequencies)
            {
                var square = turn == 1 ? player1.square : player2.square;
                if (!MemoizationCache.TryGetValue((square, rolled.Key), out var newSquare)) {
                    newSquare = ((square + rolled.Key - 1) % 10) + 1;
                    MemoizationCache.Add((square, rolled.Key), newSquare);
                }
                var p1 = turn == 1 ? (newSquare, player1.score + newSquare) : (player1.square, player1.score);
                var p2 = turn == 0 ? (newSquare, player2.score + newSquare) : (player2.square, player2.score);

                if (p1.Item2 >= 21) wins.p1Wins += rolled.Value;
                else if (p2.Item2 >= 21) wins.p2Wins += rolled.Value;
                else
                {
                    var w = PlayTurn(p1, p2, 1 - turn);
                    wins.p1Wins += rolled.Value * w.player1Wins;
                    wins.p2Wins += rolled.Value * w.player2Wins;
                }
            }
            return wins;
        }

        private void ParseInput()
        {
            PlayerOne = (int.Parse(InputLines.First().Last().ToString()), 0);
            PlayerTwo = (int.Parse(InputLines.Last().Last().ToString()), 0);
        }
    }
}
