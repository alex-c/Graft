using Graft.Primitives;
using System;

namespace Graft.Algorithms.ShortestPath
{
    public static class BellmanFordMoore
    {
        public static IWeightedGraph<TV, TW> FindShortestPath<TV, TW>(IWeightedGraph<TV, TW> graph, IVertex<TV> source, IVertex<TV> target)
            where TV : IEquatable<TV>
            where TW : IComparable
        {
            throw new NotImplementedException();
        }
    }
}
