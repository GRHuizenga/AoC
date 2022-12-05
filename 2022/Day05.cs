using Core;

namespace _2022
{
    public class Day05 : Day<string>
    {
        public Day05(int year, string fileName) : base(year, fileName)
        {
        }

        public override string PartOne()
        {
            return Solve(new CrateMover9000());
        }

        public override string PartTwo()
        {
            return Solve(new CrateMover9001());
        }

        private string Solve(Strategy strategy)
        {
            var stacks = CreateInitialStacks();
            foreach (var move in InputLines.Where(move => move.StartsWith("move")))
            {
                var digits = move.NumbersInString();
                strategy.Move(digits[0], stacks[digits[1] - 1], stacks[digits[2] - 1]);
            }
            return string.Join("", stacks.Select(stack => stack.Pop()));
        }

        private Stack<char>[] CreateInitialStacks() =>
            InputLines
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Transpose()
                .Select(stack => string.Join("", stack).Trim())
                .Where(stack => !string.IsNullOrEmpty(stack) && char.IsLetter(stack.First()))
                .Select(Day05Extensions.CreateStack)
                .ToArray();

        private abstract class Strategy
        {
            public abstract void Move(int numberOfCrates, Stack<char> from, Stack<char> to);
        }

        private class CrateMover9000 : Strategy
        {
            public override void Move(int numberOfCrates, Stack<char> from, Stack<char> to)
            {
                foreach (var i in Enumerable.Range(0, numberOfCrates))
                {
                    to.Push(from.Pop());
                }
            }
        }

        private class CrateMover9001 : Strategy
        {
            public override void Move(int numberOfCrates, Stack<char> from, Stack<char> to)
            {
                var tempStack = new Stack<char>();
                foreach (var i in Enumerable.Range(0, numberOfCrates))
                {
                    tempStack.Push(from.Pop());
                }
                foreach (var crate in tempStack)
                {
                    to.Push(crate);
                }
            }
        }
    }

    public static class Day05Extensions
    {
        public static Stack<char> CreateStack(this string @this)
        {
            return new Stack<char>(@this.ToCharArray().Reverse().Skip(1));
        }
    }
}
