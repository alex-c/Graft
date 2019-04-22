using Graft.Algorithms.Search;
using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Algorithms
{
    public static class ConnectedComponents
    {
        public static int Count<T>(IGraph<T> graph) where T: IEquatable<T>
        {
            HashSet<T> visitedVerteces = new HashSet<T>();
            IVertex<T> nextComponent = graph.GetFirstVertex();
            int connectedComponents = 0;

            while (nextComponent != null)
            {
                BreadthFirstSearch.Traverse(graph, nextComponent, v => visitedVerteces.Add(v.Value));
                connectedComponents++;
                nextComponent = graph.GetFirstMatchingVertex(v => !visitedVerteces.Contains(v.Value));
            }

            return connectedComponents;
        }
    }
}
