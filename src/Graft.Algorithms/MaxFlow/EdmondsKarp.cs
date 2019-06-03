using Graft.Primitives;
using System;

namespace Graft.Algorithms.MaxFlow
{
    public static class EdmondsKarp
    {
        public static IWeightedGraph<TV, TW> FindMaxFlow<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target) where TV : IEquatable<TV>
        {
            throw new NotImplementedException("TODO: implement this!");
        }
    }
}
