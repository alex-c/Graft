using System;
using System.Collections.Generic;
using System.Text;

namespace Graft.Algorithms.MinCostFlow
{
    public static class SuccessiveShortestPath
    {
        public static IWeightedGraph<TV, TW> FindCostMinimalFlow<TV, TW>(IWeightedGraph<TV, TW> graph,
            TV superSourceValue,
            TV superTargetValue,
            Func<TW, TW, TW> combineValues,
            Func<TW, TW, TW> substractValues,
            Func<TW, TW> negateValue,
            TW zeroValue,
            TW maxValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            throw new NotImplementedException("TODO");
        }
    }
}
