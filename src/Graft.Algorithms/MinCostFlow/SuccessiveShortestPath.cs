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
            // Add initial flow values to all graph edges: 0 or maximum edge capacity for edges with negative costs
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                if (edge.GetAttribute<TW>(Constants.COSTS).CompareTo(zeroValue) < 0)
                {
                    edge.SetAttribute(Constants.FLOW, edge.Weight);
                }
                else
                {
                    edge.SetAttribute(Constants.FLOW, zeroValue);
                }
            }

            // Set pseudo-balances and track pseudo-nodes
            List<IVertex<TV>> pseudoSources = new List<IVertex<TV>>();
            List<IVertex<TV>> pseudoTargets = new List<IVertex<TV>>();
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                // Compute pseudo balances
                UpdatePseudoBalance(graph, vertex, combineValues, substractValues, zeroValue);

                // Track pseudo-sources and pseudo-targets
                if (vertex.GetAttribute<TW>(Constants.BALANCE).CompareTo(vertex.GetAttribute<TW>(Constants.PSEUDO_BALANCE)) > 0)
                {
                    pseudoSources.Add(vertex);
                }
                else if (vertex.GetAttribute<TW>(Constants.BALANCE).CompareTo(vertex.GetAttribute<TW>(Constants.PSEUDO_BALANCE)) < 0)
                {
                    pseudoTargets.Add(vertex);
                }
            }
            
            while (pseudoSources.Any() && pseudoTargets.Any())
            {
                // Build residual graph
                IWeightedGraph<TV, TW> residualGraph = CycleCanceling.BuildResidualGraph(graph, substractValues, negateValue, zeroValue);

                // Select pseudo-source
                IVertex<TV> pseudoSource = pseudoSources.First();

                // Attempt to find a pseudo-target reachable from the pseudo-source in the residual graph
                IVertex<TV> pseudoTarget = null;
                IWeightedGraph<TV, TW> pathToTarget = null;
                foreach (IVertex<TV> currentPseudoTarget in pseudoTargets)
                {
                    try
                    {
                        IWeightedGraph<TV, TW> bfmGraph = BuildGraphForBellmanFordMoore(residualGraph);
                        IVertex<TV> bfmSource = bfmGraph.GetFirstMatchingVertex(v => v.Value.Equals(pseudoSource.Value));
                        IVertex<TV> bfmTarget = bfmGraph.GetFirstMatchingVertex(v => v.Value.Equals(currentPseudoTarget.Value));
                        IWeightedGraph<TV, TW> pathToCurrentTarget = BellmanFordMoore.FindShortestPath(bfmGraph, bfmSource, bfmTarget, zeroValue, maxValue, combineValues);
                        if (pathToCurrentTarget != null)
                        {
                            pseudoTarget = currentPseudoTarget;
                            pathToTarget = pathToCurrentTarget;
                            break;
                        }
                    }
                    catch(Exception) { /* silent */ }
                }

                // Abort if no pair of reachable pseudo-nodes was found
                if (pseudoTarget == null)
                {
                    break;
                }

                // Determine max possible augmenting flow value
                TW minPathCapacity = maxValue;
                foreach (IWeightedEdge<TV, TW> edge in pathToTarget.GetAllEdges())
                {
                    if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                    {
                        IWeightedEdge<TV, TW> residualEdge = residualGraph.GetEdgeBetweenVerteces(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value);
                        minPathCapacity = MinValue(new TW[] { minPathCapacity, residualEdge.Weight });
                    }
                    else
                    {
                        throw new GraphNotDirectedException();
                    }
                }
                TW sourceRestBalance = substractValues(pseudoSource.GetAttribute<TW>(Constants.BALANCE), pseudoSource.GetAttribute<TW>(Constants.PSEUDO_BALANCE));
                TW targetRestBalance = substractValues(pseudoTarget.GetAttribute<TW>(Constants.PSEUDO_BALANCE), pseudoTarget.GetAttribute<TW>(Constants.BALANCE));
                TW augmentingFlow = MinValue(new TW[] { minPathCapacity, sourceRestBalance, targetRestBalance });

                // Update b-flow in original graph
                foreach (IWeightedEdge<TV, TW> edge in pathToTarget.GetAllEdges())
                {
                    if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                    {
                        IWeightedEdge<TV, TW> residualEdge = residualGraph.GetEdgeBetweenVerteces(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value);
                        if (residualEdge.GetAttribute<EdgeDirection>(Constants.DIRECTION) == EdgeDirection.Forward)
                        {
                            IWeightedEdge<TV, TW> graphEdge = graph.GetEdgeBetweenVerteces(directedEdge.OriginVertex.Value, directedEdge.TargetVertex.Value);
                            graphEdge.SetAttribute(Constants.FLOW, combineValues(graphEdge.GetAttribute<TW>(Constants.FLOW), augmentingFlow));
                        }
                        else
                        {
                            IWeightedEdge<TV, TW> graphEdge = graph.GetEdgeBetweenVerteces(directedEdge.TargetVertex.Value, directedEdge.OriginVertex.Value);
                            graphEdge.SetAttribute(Constants.FLOW, substractValues(graphEdge.GetAttribute<TW>(Constants.FLOW), augmentingFlow));
                        }
                    }
                    else
                    {
                        throw new GraphNotDirectedException();
                    }
                }

                // Remove pseudo-nodes (from tracking) if they will have their balance satisfied
                if (augmentingFlow.Equals(sourceRestBalance))
                {
                    pseudoSources.Remove(pseudoSource);
                }
                if (augmentingFlow.Equals(targetRestBalance))
                {
                    pseudoTargets.Remove(pseudoTarget);
                }

                // Update pseudo-balances of used pseudo-source and pseudo-target
                UpdatePseudoBalance(graph, pseudoSource, combineValues, substractValues, zeroValue);
                UpdatePseudoBalance(graph, pseudoTarget, combineValues, substractValues, zeroValue);
            }

            // Check balances
            if (!graph.GetAllVerteces().All(v => v.GetAttribute<TW>(Constants.BALANCE).Equals(v.GetAttribute<TW>(Constants.PSEUDO_BALANCE))))
            {
                throw new NoBFlowException();
            }

            // If we reach this, a valid b-flow was found!
            return graph;
        }

        private static void UpdatePseudoBalance<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> vertex,
            Func<TW, TW, TW> combineValues,
            Func<TW, TW, TW> substractValues,
            TW zeroValue)
            where TV: IEquatable<TV> where TW :IComparable
        {
            TW pseudoBalance = zeroValue;

            // Add flow values of outgoing edges
            foreach (IWeightedEdge<TV, TW> edge in graph.GetEdgesOfVertex(vertex))
            {
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    pseudoBalance = combineValues(pseudoBalance, edge.GetAttribute<TW>(Constants.FLOW));
                }
                else
                {
                    throw new GraphNotDirectedException();
                }
            }

            // Add flow values of incoming edges
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                if (edge is IWeightedDirectedEdge<TV, TW> directedEdge)
                {
                    if (directedEdge.TargetVertex == vertex)
                    {
                        pseudoBalance = substractValues(pseudoBalance, edge.GetAttribute<TW>(Constants.FLOW));
                    }
                }
                else
                {
                    throw new GraphNotDirectedException();
                }
            }
            vertex.SetAttribute(Constants.PSEUDO_BALANCE, pseudoBalance);
        }

        private static IWeightedGraph<TV, TW> BuildGraphForBellmanFordMoore<TV, TW>(IWeightedGraph<TV, TW> graph)
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

            // Build and return result graph
            return builder.Build();
        }

        private static TW MinValue<TW>(IEnumerable<TW> values) where TW : IComparable
        {
            if (values.Any())
            {
                TW result = values.First();
                for (int i = 1; i < values.Count(); i++)
                {
                    TW currentValue = values.ElementAt(i);
                    if (currentValue.CompareTo(result) < 0)
                    {
                        result = currentValue;
                    }
                }
                return result;
            }
            else
            {
                throw new InvalidOperationException("Can't find minimum of no values!");
            }
        }
    }
}
