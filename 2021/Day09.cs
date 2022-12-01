using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public class Day09 : Day<int>
    {
        private int[] HeightMap;
        private IEnumerable<int> Sinks;
        private int Size;

        public Day09() : base(2021, "day09")
        {
            Size = InputLines.First().Count();
            HeightMap = InputLines.SelectMany(c => c.ToCharArray().Select(d => int.Parse(d.ToString()))).ToArray();
            Sinks = Enumerable.Range(0, HeightMap.Length)
                .Where(index1D => HeightMap[index1D] < MinNeighbour(index1D).Item2);
        }

        public override int PartOne()
        {
            return Sinks.Sum(index1D => HeightMap[index1D] + 1);
        }

        public override int PartTwo()
        {
            var basins = Enumerable.Range(0, HeightMap.Length).Select(c => -1).ToArray();
            foreach (var sink in Enumerable.Range(0, Sinks.Count())) basins[Sinks.ElementAt(sink)] = sink;
            foreach (var index1d in Enumerable.Range(0, HeightMap.Length))
            {
                if (HeightMap[index1d] != 9)
                {
                    basins[index1d] = FindSinkRecursive(basins, index1d);
                }
            }
            
            return basins
                .GroupBy(b => b)
                .Where(x => x.Key != -1)
                .OrderByDescending(b => b.Count())
                .Take(3)
                .Aggregate(1, (acc, basin) => acc * basin.Count());
        }

        private int FindSinkRecursive(int[] basins, int index1d)
        {
            return basins[index1d] != -1 ? basins[index1d] : FindSinkRecursive(basins, MinNeighbour(index1d).Item1);
        }

        private (int index1d, int height) MinNeighbour(int index1d)
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>()
                .Select(direction => direction switch
                {
                    Direction.North => index1d - Size < 0 ? (index1d, 10) : (index1d - Size, HeightMap[index1d - Size]),
                    Direction.South => index1d + Size > HeightMap.Length - 1 ? (index1d, 10) : (index1d + Size, HeightMap[index1d + Size]),
                    Direction.East => (index1d + 1) % Size == 0 ? (index1d, 10) : (index1d + 1, HeightMap[index1d + 1]),
                    Direction.West => index1d - 1 < 0 || (index1d - 1) % Size == Size - 1 ? (index1d, 10) : (index1d - 1, HeightMap[index1d - 1]),
                    _ => throw new Exception("This should never happen..."),
                })
                .OrderBy(c => c.Item2)
                .First();
        }
    }

    public enum Direction
    {
        North,
        East,
        South,
        West,
    }
}
