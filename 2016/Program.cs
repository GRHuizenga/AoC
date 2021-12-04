using _2016.Days;
using System;

namespace _2016
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var day = new Day01();
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
