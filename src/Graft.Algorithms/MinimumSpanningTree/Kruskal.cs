using Graft.Default;
using Graft.Primitives;
using System.Linq;
using System.Collections.Generic;
using Graft.Algorithms.Search;
using System;
using Graft.DataStructures;

namespace Graft.Algorithms.MinimumSpanningTree
{
    public static class Kruskal
    {
        public static IWeightedGraph<TV, TW> FindMinimumSpanningTree<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            // Builder used to incrementally build the target minimum spanning tree
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            // Get all verteces and edges of graph
            IEnumerable<TV> vertexValues = graph.GetAllVerteces().Select(v => v.Value);
            IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetAllEdges();

            // Initialize disjoint set
            DisjointSet<TV> disjointSet = new DisjointSet<TV>(vertexValues);

            // Add original forest of verteces
            builder.AddVerteces(vertexValues);

            // Sorted collection of edges
            IOrderedEnumerable<IWeightedEdge<TV, TW>> sortedEdges = edges.OrderBy(e => e.Weight);

            // Apply Krsukal
            foreach (IWeightedEdge<TV, TW> edge in sortedEdges)
            {

                // Select origin and target verteces
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

                // Find sets of origin and target verteces
                TV originSet = disjointSet.FindSet(originVertex.Value);
                TV targetSet = disjointSet.FindSet(targetVertex.Value);

                // If the sets are disjoint, perform a union operation and add the edge to the target graph!
                if (!originSet.Equals(targetSet))
                {
                    disjointSet.Union(originSet, targetSet);
                    builder.AddEdge(originVertex.Value, targetVertex.Value, edge.Weight);
                }
            }

            // No verteces left to check, build and return target graph
            return builder.Build();
        }
    }
}
