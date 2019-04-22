using System;

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
            throw new NotImplementedException();
        }
    }
}
