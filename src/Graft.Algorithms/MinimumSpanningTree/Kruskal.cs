using Graft.Default;
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
            throw new NotImplementedException();
            //GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            //// Get all verteces and edges of graph
            //IEnumerable<IVertex<TV>> verteces = graph.GetVerteces();
            //IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetEdges();

            //// Forest of verteces
            //builder.AddVerteces(verteces.Select(v => v.Value));

            //// Sorted collection of edges
            //HashSet<IWeightedEdge<TV, TW>> sortedEdges = new HashSet<IWeightedEdge<TV, TW>>(edges.OrderBy(e => e.Weight));

            //// Apply Krsukal
            //IWeightedGraph<TV, TW> tempGraph = null;
            //while (sortedEdges.Any())
            //{
            //    tempGraph = builder.Build();
            //    IWeightedEdge<TV, TW> nextEdge = sortedEdges.First();
            //    sortedEdges.Remove(nextEdge);

            //    BreadthFirstSearch.Search(tempGraph, nextEdge.OriginVertex, v => { if (v.Value == nextEdge.TargetVertex.Value) { } });
            //}
        }
    }
}
