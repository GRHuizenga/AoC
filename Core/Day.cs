using System.Collections.Generic;

namespace Core
{
    public abstract class Day<T>
    {
        private readonly int Year;
        private readonly string FileName;

        protected string Input => System.IO.File.ReadAllText($@"C:\AoC\{Year}\PuzzleInputs\{FileName}.txt");
        protected IEnumerable<string> InputLines => System.IO.File.ReadLines($@"C:\AoC\{Year}\PuzzleInputs\{FileName}.txt");

        protected Day(int year, string fileName)
        {
            Year = year;
            FileName = fileName;
        }

        public abstract T PartOne();

        public abstract T PartTwo();
    }
}
