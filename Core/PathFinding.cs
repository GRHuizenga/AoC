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
    }
}
