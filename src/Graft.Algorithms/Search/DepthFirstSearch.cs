using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Search
{
    public static class DepthFirstSearch
    {
        public static void Search<T>(IGraph<T> graph, Action<IVertex<T>> action)
        {
            IVertex<T> firstVertex = graph.GetFirstVertex();
            Search<T>(graph, firstVertex, action);
        }

        public static void Search<T>(IGraph<T> graph, IVertex<T> startingVertex, Action<IVertex<T>> action)
        {
            Stack<IVertex<T>> verteces = new Stack<IVertex<T>>();
            HashSet<T> visited = new HashSet<T>();

            verteces.Push(startingVertex);

            while (verteces.Any())
            {
                // Get next vertex to visit
                IVertex<T> vertex = verteces.Pop();

                // Mark vertex as visited
                visited.Add(vertex.Value);

                // Apply action
                action(vertex);

                // Enqueue adjacent verteces that have not been visited yet
                foreach (IVertex<T> adjacentVertex in graph.GetAdjacentVerteces(vertex).Reverse())
                {
                    if (!visited.Contains(adjacentVertex.Value) && !verteces.Contains(adjacentVertex))
                    {
                        verteces.Push(adjacentVertex);
                    }
                }
            }
        }
    }
}
