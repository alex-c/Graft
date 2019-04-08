using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Search
{
    public static class BreadthFirstSearch
    {
        public static void Search<T>(IGraph<T> graph, Action<IVertex<T>> action)
        {
            Queue<IVertex<T>> verteces = new Queue<IVertex<T>>();
            List<T> visited = new List<T>();

            verteces.Enqueue(graph.GetFirstVertex());

            while (verteces.Any())
            {
                // Get next vertex to visit
                IVertex<T> vertex = verteces.Dequeue();

                // Mark vertex as visited
                visited.Add(vertex.Value);

                // Apply action
                action(vertex);

                // Enqueue adjacent verteces that have not been visited yet
                foreach (IVertex<T> adjacentVertex in graph.GetAdjacentVerteces(vertex))
                {
                    if (!visited.Contains(adjacentVertex.Value))
                    {
                        verteces.Enqueue(adjacentVertex);
                    }
                }
            }
        }
    }
}
