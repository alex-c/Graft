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
            IVertex<TV> target) where TV : IEquatable<TV>
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
                if (TryFindPath(residualGraph, out IEnumerable<IWeightedDirectedEdge<TV, TW>> path))
                {
                    // Augment f according to the found path
                    TW augmentingFlow = path.Min(e => e.Weight);
                    foreach (IWeightedDirectedEdge<TV, TW> edge in path)
                    {
                        if ((string)edge.GetAttribute("direction") == "forward")
                        {
                            IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)
                                graph.GetEdgeBetweenVerteces(edge.OriginVertex, edge.TargetVertex);
                            graphEdge.SetAttribute(ATTR_FLOW, (TW)graphEdge.GetAttribute(ATTR_FLOW) + augmentingFlow); // TODO: introduce combination function
                        }
                    }
                }
                else
                {
                    // Terminate if there is no (s,t)-path left in the residual graph
                    anyPathLeft = false;
                }
            } while (anyPathLeft);

            // TODO: think about return type...
            throw new NotImplementedException("TODO: implement this!");
        }

        public static IWeightedGraph<TV, TW> BuildResidualGraph<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            // TODO: implement BuildResidualGraph
            throw new NotImplementedException("TODO: implement this!");
        }

        public static bool TryFindPath<TV, TW>(IWeightedGraph<TV, TW> graph,
            out IEnumerable<IWeightedDirectedEdge<TV, TW>> path) where TV : IEquatable<TV>
        {
            // TODO: implement TryFindPath
            throw new NotImplementedException("TODO: implement this!");
        }
    }
}
