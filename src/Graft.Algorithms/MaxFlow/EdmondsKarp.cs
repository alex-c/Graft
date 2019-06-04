using Graft.Default;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.MaxFlow
{
    public static class EdmondsKarp
    {
        private static readonly string ATTR_FLOW = "flow";
        private static readonly string ATTR_DIRECTION = "direction";

        public static IWeightedGraph<TV, TW> FindMaxFlow<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target,
            Func<TW, TW, TW> combineFlowValues,
            Func<TW, TW, TW> substractFlowValues) where TV : IEquatable<TV>
        {
            // Set initial flow to 0
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                edge.SetAttribute(ATTR_FLOW, 0);
            }

            // Augment flow until there are not augmenting paths in the residual graph left
            IWeightedGraph<TV, TW> residualGraph = null;
            bool anyPathLeft = true;
            do
            {
                // Build residual graph
                residualGraph = BuildResidualGraph(graph);

                // Find (s,t)-path in residual graph
                if (TryFindPath(residualGraph, source, target, out IEnumerable<IWeightedDirectedEdge<TV, TW>> path))
                {
                    // Augment f according to the found path
                    TW augmentingFlowValue = path.Min(e => e.Weight);
                    foreach (IWeightedDirectedEdge<TV, TW> edge in path)
                    {
                        // Get current flow of edge
                        IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)
                            graph.GetEdgeBetweenVerteces(edge.OriginVertex, edge.TargetVertex);
                        TW currentFlow = (TW)graphEdge.GetAttribute(ATTR_FLOW);

                        // Add or substract augmenting flow value from current value depending on edge direction
                        if (graphEdge.GetAttribute<EdgeDirection>(ATTR_DIRECTION) == EdgeDirection.Forward)
                        {
                            graphEdge.SetAttribute(ATTR_FLOW, combineFlowValues(currentFlow, augmentingFlowValue));
                        }
                        else
                        {
                            graphEdge.SetAttribute(ATTR_FLOW, substractFlowValues(currentFlow, augmentingFlowValue));
                        }
                    }
                }
                else
                {
                    // Terminate if there is no (s,t)-path left in the residual graph
                    anyPathLeft = false;
                }
            } while (anyPathLeft);

            // Build copy of input graph with max flow as edge weight
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(true)
                .AddVerteces(graph.GetAllVerteces().Select(v => v.Value));
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)edge;
                builder.AddEdge(graphEdge.OriginVertex.Value,
                    graphEdge.TargetVertex.Value,
                    graphEdge.GetAttribute<TW>(ATTR_FLOW));
            }
            return builder.Build();
        }

        public static IWeightedGraph<TV, TW> BuildResidualGraph<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            // TODO: implement BuildResidualGraph
            throw new NotImplementedException("TODO: implement this!");
        }

        public static bool TryFindPath<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target,
            out IEnumerable<IWeightedDirectedEdge<TV, TW>> path) where TV : IEquatable<TV>
        {
            // TODO: implement TryFindPath
            throw new NotImplementedException("TODO: implement this!");
        }

        private enum EdgeDirection
        {
            Forward,
            Backward
        }
    }
}
