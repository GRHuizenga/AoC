using Core;

namespace _2022
{
    public class Day06 : Day<int>
    {
        public Day06(int year, string fileName) : base(year, fileName)
        {
        }

        public override int PartOne()
        {
            return FindMarkerPosition(4);
        }

        public override int PartTwo()
        {
            return FindMarkerPosition(14);
        }

        private int FindMarkerPosition(int size) => Input.SlidingWindow(size).TakeWhile(window => window.ToHashSet().Count < size).Count() + size;
    }
}
