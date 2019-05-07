using Graft.Default;
using Graft.Exceptions;
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

            // Starting point
            IVertex<TV> currentVertex = source;
            builder.AddVertex(source.Value);

            // Track which verteces have already been added to the tour, and how many edges have already been used
            HashSet<TV> visitedVerteces = new HashSet<TV>() { source.Value };
            int edgesUsed = 0;

            // Find and use minimum non-used edge leading from each vertex
            while (edgesUsed < graph.VertexCount)
            {
                // Last edge, select edge back to start
                if (edgesUsed == graph.VertexCount - 1)
                {
                    try
                    {
                        IWeightedEdge<TV, TW> closingEdge = graph.GetEdgeBetweenVerteces(currentVertex, source);
                        builder.AddEdge(currentVertex.Value, source.Value, closingEdge.Weight);
                    }
                    catch (VertecesNotConnectedException<TV> exception)
                    {
                        throw new GraphNotCompleteException("The NearestNeighbor algorithm expects the input graph to be complete!", exception);
                    }
                }
                else
                {
                    IWeightedEdge<TV, TW> minEdge = graph.GetEdgesOfVertex(currentVertex)
                        .Where(e => !visitedVerteces.Contains(e.ConnectedVertex(currentVertex).Value))
                        .OrderBy(e => e.Weight)
                        .FirstOrDefault();
                    if (minEdge == null)
                    {
                        throw new GraphNotCompleteException("The NearestNeighbor algorithm expects the input graph to be complete!");
                    }

                    // Get target vertex and update current vertex
                    IVertex<TV> target = minEdge.ConnectedVertex(currentVertex);
                    currentVertex = target;

                    // Update tracking
                    visitedVerteces.Add(target.Value);

                    // Update result graph
                    builder.AddVertex(target.Value);
                    builder.AddEdge(source.Value, target.Value, minEdge.Weight);
                }
                edgesUsed++;
            }

            // Done: used n-1 edges for n verteces
            return builder.Build();
        }
    }
}
