using System.Collections.Generic;

namespace _2016.Days
{
    public abstract class Day
    {
        protected IEnumerable<string> Lines;

        public Day(string inputFile)
        {
            Lines = System.IO.File.ReadLines($@"C:\AoC\AdventOfCode\2016\Days\{inputFile}");
        }
    }
}
