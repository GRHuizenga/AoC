using _2022;

namespace _2021
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var day = new Day03(2022, "day03");
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