using Graft.Algorithms.MaxFlow;
using Graft.BalanceGraph;
using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.MinCostFlow
{
    public static class CycleCanceling
    {
        public static IWeightedGraph<TV, TW> FindCostMinimalFlow<TV, TW>(IWeightedGraph<TV, TW> graph,
            TV superSourceValue,
            TV superTargetValue,
            Func<TW, TW, TW> combineValues,
            Func<TW, TW, TW> substractValues,
            Func<TW, TW> negateValue,
            TW zeroValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            IWeightedGraph<TV, TW> graphWithSupers = BuildGraphWithSuperSourceAndTarget(graph, superSourceValue, superTargetValue, negateValue);

            IWeightedGraph<TV, TW> maxFlow = EdmondsKarp.FindMaxFlow(graphWithSupers,
                graphWithSupers.GetFirstMatchingVertex(v => v.Value.Equals(superSourceValue)),
                graphWithSupers.GetFirstMatchingVertex(v => v.Value.Equals(superTargetValue)),
                combineValues,
                substractValues,
                zeroValue);
            TW maxFlowValue = FlowValue(maxFlow, superSourceValue, combineValues, zeroValue);

            // Check for existance of a b-flow
            IEnumerable<IVertex<TV>> sources = graph.GetAllMatchingVerteces(v => v.GetAttribute<double>(Constants.BALANCE) > 0);
            TW sourcesBalance = zeroValue;
            foreach (var source in sources)
            {
                sourcesBalance = combineValues(sourcesBalance, source.GetAttribute<TW>(Constants.BALANCE));
            }
            if (maxFlowValue.Equals(sourcesBalance))
            {

                throw new NotImplementedException("TODO: implement this!");
            }
            else
            {
                throw new NoBFlowException();
            }
        }

        private static IWeightedGraph<TV, TW> BuildGraphWithSuperSourceAndTarget<TV, TW>(IWeightedGraph<TV, TW> graph, 
            TV superSourceValue,
            TV superTargetValue,
            Func<TW, TW> negateValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            // Find sources and targets in original graph, based on balance
            IEnumerable<IVertex<TV>> sources = graph.GetAllMatchingVerteces(v => v.GetAttribute<double>(Constants.BALANCE) > 0);
            IEnumerable<IVertex<TV>> targets = graph.GetAllMatchingVerteces(v => v.GetAttribute<double>(Constants.BALANCE) < 0);

            // Clone graph
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(true);
            foreach (var vertex in graph.GetAllVerteces())
            {
                builder.AddVertex(vertex.Value);
            }
            foreach (var edge in graph.GetAllEdges())
            {
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    builder.AddEdge(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value, directedEdge.Weight);
                }
                else
                {
                    throw new GraphNotDirectedException();
                }
            }

            // Add super source and target
            builder.AddVertex(superSourceValue);
            builder.AddVertex(superTargetValue);

            // Add edges from super source and to super target
            foreach (IVertex<TV> source in sources)
            {
                builder.AddEdge(superSourceValue, superTargetValue, source.GetAttribute<TW>(Constants.BALANCE));
            }
            foreach (IVertex<TV> target in targets)
            {
                builder.AddEdge(target.Value, superTargetValue, negateValue(target.GetAttribute<TW>(Constants.BALANCE)));
            }

            // Build and return result graph
            return builder.Build();
        }

        private static TW FlowValue<TV, TW>(IWeightedGraph<TV, TW> flow, TV sourceVertexValue, Func<TW, TW, TW> combineValues, TW zeroValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            TW flowValue = zeroValue;
            IVertex<TV> sourceVertex = flow.GetFirstMatchingVertex(v => v.Value.Equals(sourceVertexValue));
            foreach (var edge in flow.GetEdgesOfVertex(sourceVertex).Where(e => ((IWeightedDirectedEdge<TV, TW>)e).OriginVertex == sourceVertex))
            {
                flowValue = combineValues(flowValue, edge.Weight);
            }
            return flowValue;
        }
    }
}
