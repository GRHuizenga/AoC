using Core;

namespace _2022
{
    public class Day02 : Day<int>
    {
        public Day02(int year, string fileName) : base(year, fileName)
        {
        }

        public override int PartOne()
        {
            return Solve(new PartOneStrategy());
        }

        public override int PartTwo()
        {
            return Solve(new PartTwoStrategy());
        }

        private int Solve(Strategy strategy) => InputLines.Select(strategy.Score).Sum();
    }

    public abstract class Strategy
    {
        private readonly Dictionary<string, int> OutcomeScores = new()
        {
            ["A A"] = 4,
            ["A B"] = 8,
            ["A C"] = 3,
            ["B A"] = 1,
            ["B B"] = 5,
            ["B C"] = 9,
            ["C A"] = 7,
            ["C B"] = 2,
            ["C C"] = 6,
        };

        public virtual int Score(string game)
        {
            return OutcomeScores[game];
        }
    }

    public class PartOneStrategy : Strategy
    {
        public override int Score(string game)
        {
            return base.Score(game.Replace("X", "A").Replace("Y", "B").Replace("Z", "C"));
        }
    }

    public class PartTwoStrategy : Strategy
    {
        private readonly Dictionary<char, Func<char, char>> Outcome = new()
        {
            ['X'] = (z) => z switch
            {
                'A' => 'C',
                'B' => 'A',
                _ => 'B'
            },
            ['Y'] = (y) => y,
            ['Z'] = (x) => x switch
            {
                'A' => 'B',
                'B' => 'C',
                _ => 'A'
            },
        };

        public override int Score(string game)
        {
            var gameCharArray = game.ToCharArray();
            gameCharArray[2] = Outcome[gameCharArray[2]](gameCharArray[0]);
            return base.Score(string.Join("", gameCharArray));
        }
    }
}
