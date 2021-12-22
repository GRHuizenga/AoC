using System.Collections.Generic;

namespace Core
{
    public abstract class Day<T>
    {
        protected IEnumerable<string> Input;

        public Day(int year, string fileName)
        {
            Input = System.IO.File.ReadLines($@"D:\AdventOfCode\{year}\PuzzleInputs\{fileName}.txt");
        }

        public abstract T PartOne();

        public abstract T PartTwo();
    }
}
