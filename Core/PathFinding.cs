using Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public static class PathFinding
    {
        public static (int[] distances, bool[] prev) Dijkstra(int[] graph, int sourceIndex)
        {
            int size = (int)Math.Sqrt(graph.Length);
            int[] distance = Enumerable.Repeat(int.MaxValue, graph.Length).ToArray();
            bool[] visited = Enumerable.Repeat(false, graph.Length).ToArray();
            distance[sourceIndex] = 0;

            foreach (var vertex in Enumerable.Range(0, graph.Length))
            {
                int u = MinDistanceIndex(distance, visited);
                visited[u] = true;

                foreach (var neighbour in u.GetDownAndRightNeighbours(size))
                {
                    if (!visited[neighbour] && distance[u] + graph[neighbour] < distance[neighbour])
                    {
                        distance[neighbour] = distance[u] + graph[neighbour];
                    }
                }
            }

            return (distance, visited);
        }

        private static int MinDistanceIndex(int[] distance, bool[] visited)
        {
            return Enumerable.Range(0, distance.Length)
                .Where(index => !visited[index])
                .Select(index => (index, distance[index]))
                .OrderBy(pair => pair.Item2)
                .First().index;
        }

        public static int AStarSearch(int[,] graph, int source, int target)
        {
            var xLength = (int)Math.Sqrt(graph.Length);
            var length = xLength * 5;
            var queue = new PriorityQueue<int, int>();
            queue.Enqueue(source, 0);

            var visited = Enumerable.Repeat(false, length * length).ToArray();

            var distances = new Dictionary<int, int>() { [source] = 0 };

            while (queue.TryDequeue(out var current, out int _))
            {
                if (visited[current]) continue;

                visited[current] = true;

                if (current == target) return distances[target];

                var cost = distances[current];
                foreach (var neighbour in current.GetNeighbouringIndices(length, false).Where(n => !visited[n]))
                {
                    var nRisk = cost + GetRisk(neighbour % length, neighbour / length) ;
                    if (nRisk < distances.GetValueOrDefault(neighbour, int.MaxValue))
                    {
                        distances[neighbour] = nRisk;
                        queue.Enqueue(neighbour, nRisk);
                    }
                }
            }

            int GetRisk(int x, int y)
            {
                var ydim = y / xLength;
                var xdim = x / xLength;
                var value = graph[y % xLength, x % xLength];
                return ((value - 1 + ydim + xdim) % 9) + 1;
            }

            return distances[target];
        }
    }
}
