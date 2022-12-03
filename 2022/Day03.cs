using Core;

namespace _2022
{
    public class Day03 : Day<int>
    {
        public Day03(int year, string fileName) : base(year, fileName)
        {
        }

        public override int PartOne()
        {
            return InputLines.Select(Extensions.FindCommonItemInRucksack).Select(Extensions.DeterminePriority).Sum();
        }

        public override int PartTwo()
        {
            return InputLines.Group(3).Select(Extensions.FindCommonItemInThreeRucksacks).Select(Extensions.DeterminePriority).Sum();
        }
    }

    public static class Extensions
    {
        public static char FindCommonItemInRucksack(this string @this)
        {
            var rucksackCapacity = @this.Length / 2;
            var leftRucksack = @this.Substring(0, rucksackCapacity);
            var rightRucksack = @this.Substring(rucksackCapacity);
            return leftRucksack.Intersect(rightRucksack).First();
        }

        public static char FindCommonItemInThreeRucksacks(this IEnumerable<string> @this)
        {
            var asArray = @this.ToArray();
            return asArray[0].Intersect(asArray[1].Intersect(asArray[2])).First();
        }

        public static int DeterminePriority(this char @this)
        {
            if (char.IsLower(@this)) return @this - 96;
            return @this - 38;
        }
    }
}
