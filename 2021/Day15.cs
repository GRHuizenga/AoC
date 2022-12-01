using Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day15 : Day<int>
    {
        public Day15() : base(2021, "day15") { }

        public override int PartOne()
        {
            var caveSystem = InputLines.SelectMany(line => line.ToCharArray().Select(cave => int.Parse(cave.ToString()))).ToArray();
            return PathFinding.Dijkstra(caveSystem, 0).distances.Last();
        }

        public override int PartTwo()
        {
            var block = InputLines.Select(line => line.ToCharArray().Select(cave => int.Parse(cave.ToString())));

            var caveSystem = new int[100, 100];
            var x = 0;
            var y = 0;
            foreach (var row in block)
            {
                x = 0;
                foreach (var cave in row)
                {
                    caveSystem[y, x] = cave;
                    x++;
                }
                y++;
            }

            return PathFinding.AStarSearch(caveSystem, 0, (500 * 500) - 1);
        }
    }
}
