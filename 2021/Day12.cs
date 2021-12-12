using Core;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace _2021
{
    public class Day12 : Day<int>
    {
        private List<(string from, string to)> Connections;

        public Day12() : base(2021, "day12")
        {
            Connections = Input
                .SelectMany(connection =>
                {
                    var caves = connection.Split('-');
                    return new (string from, string to)[] { (caves[0], caves[1]), (caves[1], caves[0]) };
                })
                .Where(connection => connection.to != "start" && connection.from != "end")
                .ToList();
        }

        public override int PartOne()
        {
            return Visit("start", new List<string>(), false);
        }

        public override int PartTwo()
        {
            return Visit("start", new List<string>(), true);
        }

        private int Visit(string cave, List<string> visited, bool canVisitSmallCaveTwice, bool hasVisitedSmallCaveTwice = false)
        {
            // if end, return
            if (cave == "end") return 1;

            // mark cave as visited
            visited.Add(cave);

            return Connections
                .Where(c => c.from == cave)
                .Select(c => c.to)
                .Aggregate(0, (pathLength, connectingCave) =>
                {
                    // if we havent visited the cave yet or if it is a big cave
                    if (!visited.Contains(connectingCave) || connectingCave.ToUpper() == connectingCave)
                    {
                        // recursively visit the connecting cave
                        return pathLength + Visit(connectingCave, new List<string>(visited), canVisitSmallCaveTwice, hasVisitedSmallCaveTwice);
                    }
                    else if (canVisitSmallCaveTwice && !hasVisitedSmallCaveTwice && connectingCave.ToUpper() != connectingCave)
                    {
                        // we can visit a small cave only once
                        return pathLength + Visit(connectingCave, new List<string>(visited), canVisitSmallCaveTwice, true);
                    }
                    return pathLength;
                });
        }
    }
}
