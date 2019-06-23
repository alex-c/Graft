using Graft.Primitives;
using System;

namespace Graft.Algorithms.MinCostFlow
{
    public static class CycleCanceling
    {
        public static IWeightedGraph<TV, TW> FindCostMinimalFlow<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target) where TV : IEquatable<TV> where TW : IComparable
        {
            throw new NotImplementedException("TODO: implement this!");
        }
    }
}
