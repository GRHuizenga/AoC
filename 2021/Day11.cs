using Core;
using System.Linq;

namespace _2021
{
    public class Day11 : Day<int>
    {
        private (int energyLevel, bool flashed)[] Octopuses;

        public Day11() : base(2021, "day11") { }

        public override int PartOne()
        {
            ParseInput();
            return Enumerable.Range(0, 100).Select(step => ModelEnergyLevels()).Sum();
        }

        public override int PartTwo()
        {
            ParseInput();
            var count = 0;
            while (ModelEnergyLevels() != 100) count++;
            return ++count;
        }

        private void ParseInput()
        {
            Octopuses = Input
                .SelectMany(line => line.ToCharArray().Select(octopus => (int.Parse(octopus.ToString()), false)))
                .ToArray();
        }

        private int ModelEnergyLevels()
        {
            // step 1: increment energy levels
            Octopuses = Octopuses.Select(octopus => (++octopus.energyLevel, octopus.flashed)).ToArray();

            // step 2: flash while energy level > 9 and not flashed
            do
            {
                foreach (var index in Enumerable.Range(0, Octopuses.Length).Where(i => !Octopuses[i].flashed && Octopuses[i].energyLevel > 9))
                {
                    Octopuses[index].flashed = true;
                    foreach (var neighbour in index.GetNeighbouringIndices(10))
                    {
                        Octopuses[neighbour].energyLevel++;
                    }
                }
            } while (Octopuses.Any(octopus => octopus.energyLevel > 9 && !octopus.flashed));

            // step 3: set all flased to false and energy levels > 9 to 0
            Octopuses = Octopuses.Select(octopus => (octopus.flashed ? 0 : octopus.energyLevel, false)).ToArray();

            // step 4: return the number of flashes
            return Octopuses.Count(octopus => octopus.energyLevel == 0);
        }
    }
}
