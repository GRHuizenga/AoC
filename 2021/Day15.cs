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
        public Day15() : base(2021, "test") { }

        public override int PartOne()
        {
            var caveSystem = Input.SelectMany(line => line.ToCharArray().Select(cave => int.Parse(cave.ToString()))).ToArray();
            return PathFinding.Dijkstra(caveSystem, 0).distances.Last();
        }

        public override int PartTwo()
        {
            var block = Input.Select(line => line.ToCharArray().Select(cave => int.Parse(cave.ToString())));

            block = block.Select(row =>
            {
                var temp = row.ToArray();
                for (int i = 1; i <= 4; i++)
                {
                    row = row.Concat(temp.Select(t => t + i > 9 ? 1 : t + i)).ToList();
                }
                return row;
            });

            block = block.Transpose().Select(row =>
            {
                var temp = row.ToArray();
                for (int i = 1; i <= 4; i++)
                {
                    row = row.Concat(temp.Select(t => t + i > 9 ? 1 : t + i)).ToList();
                }
                return row;
            }).Transpose();

            var caveSystem = block.SelectMany(row => row).ToArray();

            var x = PathFinding.Dijkstra(caveSystem, 0).distances.Last();

            return -1;
        }
    }
}
