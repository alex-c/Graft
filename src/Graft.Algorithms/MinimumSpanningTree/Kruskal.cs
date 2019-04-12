﻿using Graft.Default;
using Graft.Primitives;
using System.Linq;
using System.Collections.Generic;
using Graft.Algorithms.Search;
using System;

namespace Graft.Algorithms.MinimumSpanningTree
{
    public static class Kruskal
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
            Queue<IWeightedEdge<TV, TW>> sortedEdges = new Queue<IWeightedEdge<TV, TW>>(edges.OrderBy(e => e.Weight));

            // Apply Krsukal
            IWeightedGraph<TV, TW> tempGraph = null;
            while (sortedEdges.Any())
            {
                // Get most cost-efficient remaining edge
                IWeightedEdge<TV, TW> nextEdge = sortedEdges.Dequeue();

                // Check whether the verteces connected by this edge are already connected in the target graph
                tempGraph = builder.Build();
                if (BreadthFirstSearch.Search(tempGraph, nextEdge.OriginVertex, v => v.Value.Equals(nextEdge.TargetVertex.Value)) == null)
                {
                    // Verteces are not connected yet, add edge to target graph
                    builder.AddEdge(nextEdge.OriginVertex.Value, nextEdge.TargetVertex.Value, nextEdge.Weight);
                }
            }

            // No verteces left to check, build and return target graph
            return builder.Build();
        }
    }
}
