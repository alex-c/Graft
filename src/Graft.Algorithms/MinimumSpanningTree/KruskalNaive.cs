using Graft.Default;
using Graft.Primitives;
using System.Linq;
using System.Collections.Generic;
using Graft.Algorithms.Search;
using System;

namespace Graft.Algorithms.MinimumSpanningTree
{
    public static class KruskalNaive
    {
        public static IWeightedGraph<TV, TW> FindMinimumSpanningTree<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            // Builder used to incrementally build the target minimum spanning tree
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            // Get all verteces and edges of graph
            IEnumerable<IVertex<TV>> verteces = graph.GetAllVerteces();
            IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetAllEdges();

            // Add original forest of verteces
            builder.AddVerteces(verteces.Select(v => v.Value));

            // Sorted collection of edges
            IOrderedEnumerable<IWeightedEdge<TV, TW>> sortedEdges = edges.OrderBy(e => e.Weight);

            // Apply Krsukal
            IWeightedGraph<TV, TW> tempGraph = null;
            foreach (IWeightedEdge<TV, TW> edge in sortedEdges)
            {
                // Check whether the verteces connected by this edge are already connected in the target graph
                tempGraph = builder.Build();
                IVertex<TV> originVertex = null;
                IVertex<TV> targetVertex = null;
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    originVertex = directedEdge.OriginVertex;
                    targetVertex = directedEdge.TargetVertex;
                }
                else
                {
                    originVertex = edge.Verteces.First();
                    targetVertex = edge.ConnectedVertex(originVertex);
                }
                if (BreadthFirstSearch.Search(tempGraph, originVertex, v => v.Value.Equals(targetVertex.Value)) == null)
                {
                    // Verteces are not connected yet, add edge to target graph
                    builder.AddEdge(originVertex.Value, targetVertex.Value, edge.Weight);
                }
            }

            // No verteces left to check, build and return target graph
            return builder.Build();
        }
    }
}
