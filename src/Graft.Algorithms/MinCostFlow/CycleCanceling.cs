using Graft.Algorithms.MaxFlow;
using Graft.Algorithms.ShortestPath;
using Graft.BalanceGraph;
using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using static Graft.Algorithms.MaxFlow.EdmondsKarp;

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
            TW zeroValue,
            TW maxValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            // Build graph with super nodes
            IWeightedGraph<TV, TW> graphWithSuperNodes = BuildGraphWithSuperNodes(graph, superSourceValue, superTargetValue, negateValue);

            // Get max flow in graph with super nodes
            IWeightedGraph<TV, TW> maxFlow = EdmondsKarp.FindMaxFlow(graphWithSuperNodes,
                graphWithSuperNodes.GetFirstMatchingVertex(v => v.Value.Equals(superSourceValue)),
                graphWithSuperNodes.GetFirstMatchingVertex(v => v.Value.Equals(superTargetValue)),
                combineValues,
                substractValues,
                zeroValue);
            TW maxFlowValue = FlowValue(maxFlow, superSourceValue, combineValues, zeroValue);

            // Check for existance of a b-flow
            IEnumerable<IVertex<TV>> sources = graph.GetAllMatchingVerteces(v => v.GetAttribute<double>(Constants.BALANCE) > 0);
            TW sourcesBalance = zeroValue;
            foreach (IVertex<TV> source in sources)
            {
                sourcesBalance = combineValues(sourcesBalance, source.GetAttribute<TW>(Constants.BALANCE));
            }
            if (maxFlowValue.Equals(sourcesBalance))
            {
                // Copy flow values from graph with super nodes to original graph
                foreach (IWeightedEdge<TV, TW> edge in graphWithSuperNodes.GetAllEdges())
                {
                    if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                    {
                        if (!directedEdge.OriginVertex.Value.Equals(superSourceValue) &&
                            !directedEdge.TargetVertex.Value.Equals(superTargetValue))
                        {
                            graph.GetEdgeBetweenVerteces(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value)
                                .SetAttribute(Constants.FLOW, edge.GetAttribute<TW>(Constants.FLOW));
                        }
                    }
                    else
                    {
                        throw new GraphNotDirectedException();
                    }
                }

                bool modifyingCycleLeft;
                do
                {
                    // Build residual graph
                    IWeightedGraph<TV, TW> residualGraph = BuildResidualGraph(graph, substractValues, negateValue, zeroValue);

                    // Prepare graph for BellmanFordMoore
                    IWeightedGraph<TV, TW> bfmGraph = BuildGraphForBellmanFordMoore(residualGraph, superSourceValue, zeroValue);

                    // Attempt to find modifying cycle
                    IVertex<TV> bfmSuperSource = bfmGraph.GetFirstMatchingVertex(v => v.Value.Equals(superSourceValue));
                    if (modifyingCycleLeft = BellmanFordMoore.TryFindNegativeCycle(bfmGraph,
                        bfmSuperSource,
                        zeroValue,
                        maxValue,
                        combineValues,
                        out IEnumerable<IWeightedDirectedEdge<TV, TW>> cycle))
                    {
                        // Get minimum capacity of the cycle in the residual graph
                        List<IWeightedDirectedEdge<TV, TW>> rgCycle = new List<IWeightedDirectedEdge<TV, TW>>();
                        foreach (IWeightedDirectedEdge<TV, TW> edge in cycle)
                        {
                            rgCycle.Add((IWeightedDirectedEdge<TV, TW>)residualGraph.GetEdgeBetweenVerteces(edge.OriginVertex.Value, edge.TargetVertex.Value));
                        }
                        TW minCycleCapacity = rgCycle.Min(e => e.Weight);
                        
                        // Modify b-flow along cycle
                        foreach (IWeightedDirectedEdge<TV, TW> edge in rgCycle)
                        {
                            if (edge.GetAttribute<EdgeDirection>(Constants.DIRECTION) == EdgeDirection.Forward)
                            {
                                IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)graph.GetEdgeBetweenVerteces(edge.OriginVertex.Value, edge.TargetVertex.Value);
                                graphEdge.SetAttribute(Constants.FLOW, combineValues(graphEdge.GetAttribute<TW>(Constants.FLOW), minCycleCapacity));
                            }
                            else
                            {
                                IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)graph.GetEdgeBetweenVerteces(edge.TargetVertex.Value, edge.OriginVertex.Value);
                                graphEdge.SetAttribute(Constants.FLOW, substractValues(graphEdge.GetAttribute<TW>(Constants.FLOW), minCycleCapacity));
                            }
                        }
                    }
                }
                while (modifyingCycleLeft);

                // Return same graph
                return graph;
            }
            else
            {
                throw new NoBFlowException();
            }
        }

        private static IWeightedGraph<TV, TW> BuildGraphWithSuperNodes<TV, TW>(IWeightedGraph<TV, TW> graph, 
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
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                builder.AddVertex(vertex.Value);
            }
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
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
                builder.AddEdge(superSourceValue, source.Value, source.GetAttribute<TW>(Constants.BALANCE));
            }
            foreach (IVertex<TV> target in targets)
            {
                builder.AddEdge(target.Value, superTargetValue, negateValue(target.GetAttribute<TW>(Constants.BALANCE)));
            }

            // Build and return result graph
            return builder.Build();
        }

        private static TW FlowValue<TV, TW>(IWeightedGraph<TV, TW> flow,
            TV sourceVertexValue,
            Func<TW, TW, TW> combineValues,
            TW zeroValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            TW flowValue = zeroValue;
            IVertex<TV> sourceVertex = flow.GetFirstMatchingVertex(v => v.Value.Equals(sourceVertexValue));
            foreach (IWeightedEdge<TV, TW> edge in flow.GetEdgesOfVertex(sourceVertex).Where(e => ((IWeightedDirectedEdge<TV, TW>)e).OriginVertex == sourceVertex))
            {
                flowValue = combineValues(flowValue, edge.Weight);
            }
            return flowValue;
        }

        public static IWeightedGraph<TV, TW> BuildResidualGraph<TV, TW>(IWeightedGraph<TV, TW> graph,
            Func<TW, TW, TW> substractValues,
            Func<TW, TW> negateValue,
            TW zeroValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            // Build residual graph as done in Edmonds-Karp
            IWeightedGraph<TV, TW> residualGraph = EdmondsKarp.BuildResidualGraph(graph, substractValues, zeroValue);

            // Add costs
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    TW costs = edge.GetAttribute<TW>(Constants.COSTS);

                    // Add costs to forward edges
                    if (residualGraph.AreVertecesConnected(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value))
                    {
                        residualGraph.GetEdgeBetweenVerteces(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value)
                            .SetAttribute(Constants.COSTS, costs);
                    }

                    // Add negative costs to backward edges
                    if (residualGraph.AreVertecesConnected(directedEdge.TargetVertex.Value, directedEdge.OriginVertex.Value))
                    {
                        residualGraph.GetEdgeBetweenVerteces(directedEdge.TargetVertex.Value, directedEdge.OriginVertex.Value)
                            .SetAttribute(Constants.COSTS, negateValue(costs));
                    }
                }
                else
                {
                    throw new GraphNotDirectedException();
                }
            }

            // Done - return residual graph
            return residualGraph;
        }

        private static IWeightedGraph<TV, TW> BuildGraphForBellmanFordMoore<TV, TW>(IWeightedGraph<TV, TW> graph,
            TV superSourceValue,
            TW zeroValue)
            where TV : IEquatable<TV> where TW : IComparable
        {
            // Clone graph with but set costs as weights
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(true);
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                builder.AddVertex(vertex.Value);
            }
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    builder.AddEdge(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value, directedEdge.GetAttribute<TW>(Constants.COSTS));
                }
                else
                {
                    throw new GraphNotDirectedException();
                }
            }

            // Add super source
            builder.AddVertex(superSourceValue);

            // Connect super source to all verteces, set weights of these new edges to 0
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                builder.AddEdge(superSourceValue, vertex.Value, zeroValue);
            }

            // Build and return result graph
            return builder.Build();
        }

    }
}
