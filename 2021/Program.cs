using System;

namespace _2021
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var day = new Day18();
                Console.WriteLine(day.PartOne());
                Console.WriteLine(day.PartTwo());
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ;
            }
        }
    }
}
