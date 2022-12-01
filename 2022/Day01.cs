using Core;

namespace _2022
{
    public class Day01 : Day<int>
    {
        private readonly IEnumerable<int> CaloriesPerElf;

        public Day01(int year, string fileName) : base(year, fileName)
        {
            CaloriesPerElf = Input.Split("\r\n\r\n").Select(l => l.Split("\r\n").Select(int.Parse).Sum());
        }

        public override int PartOne()
        {
            return CaloriesPerElf.Max();
        }

        public override int PartTwo()
        {
            return CaloriesPerElf.OrderByDescending(c => c).Take(3).Sum();
        }
    }
}
