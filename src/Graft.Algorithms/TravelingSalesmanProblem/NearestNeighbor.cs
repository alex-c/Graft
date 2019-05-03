using Graft.Default;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.TravelingSalesmanProblem
{
    public static class NearestNeighbor
    {
        public static IWeightedGraph<TV, TW> FindTour<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            return FindTour(graph, graph.GetFirstVertex());
        }

        public static IWeightedGraph<TV, TW> FindTour<TV, TW>(IWeightedGraph<TV, TW> graph, IVertex<TV> source) where TV : IEquatable<TV>
        {
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            // Track which verteces have already been added to the tour, and how many edges have already been used
            HashSet<TV> visitedVerteces = new HashSet<TV>();
            int edgesUsed = 0;

            // Starting point
            IVertex<TV> currentVertex = source;
            builder.AddVertex(source.Value);

            // Find and use minimum non-used edge leading from each vertex
            while (edgesUsed < graph.VertexCount - 1)
            {
                IWeightedEdge<TV, TW> minEdge = graph.GetEdgesOfVertex(currentVertex)
                    .Where(e => !visitedVerteces.Contains(e.ConnectedVertex(currentVertex).Value))
                    .OrderBy(e => e.Weight)
                    .First();
                IVertex<TV> target = minEdge.ConnectedVertex(currentVertex);
                builder.AddVertex(target.Value);
                builder.AddEdge(source.Value, target.Value);
                visitedVerteces.Add(target.Value);
            }

            // Done: used n-1 edges for n verteces
            return builder.Build();
        }
    }
}
