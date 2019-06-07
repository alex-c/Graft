using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graft.Algorithms.MinFlow
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
