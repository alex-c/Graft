using Graft.Default;
using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// An implementation of Prim's algorithm.
    /// </summary>
    public static class Prim
    {
        /// <summary>
        /// Builds the minimum spanning tree of a given graph.
        /// </summary>
        /// <typeparam name="TV">Type of the graph vertex values.</typeparam>
        /// <typeparam name="TW">Type of the graph edge weights.</typeparam>
        /// <param name="graph">The graph got whcih to build the minimum spanning tree.</param>
        /// <returns>Returns the minimum spanning tree of the given graph.</returns>
        public static IWeightedGraph<TV, TW> FindMinimumSpanningTree<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            // Builder used to incrementally build the target minimum spanning tree
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            // Get all verteces and edges of graph
            IEnumerable<IVertex<TV>> verteces = graph.GetAllVerteces();
            IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetAllEdges();
            
            // TODO: initialize priority queue with all costs set to max

            // TODO: while verteces left to add, select vertex with smallest cost
                // TODO: add vertex, update vertex cost for adjacent verteces

            throw new NotImplementedException();
        }
    }
}
