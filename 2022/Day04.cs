using Core;

namespace _2022
{
    public class Day04 : Day<int>
    {
        public Day04(int year, string fileName) : base(year, fileName)
        {
        }

        public override int PartOne()
        {
            return InputLines.Select(Core.Extensions.NumbersInString).Where(Day04Extensions.FullyContains).Count();
        }

        public override int PartTwo()
        {
            return InputLines.Select(Core.Extensions.NumbersInString).Where(Day04Extensions.Overlap).Count();
        }
    }

    public static class Day04Extensions
    {
        public static bool FullyContains(this int[] @this)
        {
            if (@this[0] >= @this[2] && @this[1] <= @this[3]) return true;
            if (@this[2] >= @this[0] && @this[3] <= @this[1]) return true;
            return false;
        }

        public static bool Overlap(this int[] @this)
        {
            return Enumerable.Range(@this[0], @this[1] - @this[0] + 1).Intersect(Enumerable.Range(@this[2], @this[3] - @this[2] + 1)).Any();
        }
    }
}
