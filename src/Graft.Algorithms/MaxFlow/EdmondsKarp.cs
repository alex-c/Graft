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
            Func<TW, TW, TW> substractFlowValues,
            TW zeroValue) where TV : IEquatable<TV> where TW : IComparable
        {
            // Set initial flow to 0
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                edge.SetAttribute(ATTR_FLOW, zeroValue);
            }

            // Augment flow until there are not augmenting paths in the residual graph left
            IWeightedGraph<TV, TW> residualGraph = null;
            bool anyPathLeft = true;
            do
            {
                // Build residual graph
                residualGraph = BuildResidualGraph(graph, substractFlowValues, zeroValue);

                // Find (s,t)-path in residual graph
                IVertex<TV> residualSource = residualGraph.GetFirstMatchingVertex(v => v.Value.Equals(source.Value));
                IVertex<TV> residualTarget = residualGraph.GetFirstMatchingVertex(v => v.Value.Equals(target.Value));
                if (TryFindPath(residualGraph, residualSource, residualTarget, out List<IWeightedEdge<TV, TW>> path))
                {
                    // Augment f according to the found path
                    TW augmentingFlowValue = path.Min(e => e.Weight);
                    foreach (IWeightedDirectedEdge<TV, TW> edge in path)
                    {
                        // Add or substract augmenting flow value from current value depending on edge direction
                        if (edge.GetAttribute<EdgeDirection>(ATTR_DIRECTION) == EdgeDirection.Forward)
                        {
                            IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)
                                graph.GetEdgeBetweenVerteces(edge.OriginVertex.Value, edge.TargetVertex.Value);
                            TW currentFlow = graphEdge.GetAttribute<TW>(ATTR_FLOW);
                            graphEdge.SetAttribute(ATTR_FLOW, combineFlowValues(currentFlow, augmentingFlowValue));
                        }
                        else
                        {
                            IWeightedDirectedEdge<TV, TW> graphEdge = (IWeightedDirectedEdge<TV, TW>)
                                graph.GetEdgeBetweenVerteces(edge.TargetVertex.Value, edge.OriginVertex.Value);
                            TW currentFlow = graphEdge.GetAttribute<TW>(ATTR_FLOW);
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

        public static IWeightedGraph<TV, TW> BuildResidualGraph<TV, TW>(IWeightedGraph<TV, TW> graph,
            Func<TW, TW, TW> substractFlowValues,
            TW zeroValue) where TV : IEquatable<TV> where TW : IComparable
        {
            Dictionary<TV, Vertex<TV>> verteces = new Dictionary<TV, Vertex<TV>>();
            foreach (var vertex in graph.GetAllVerteces())
            {
                verteces.Add(vertex.Value, new Vertex<TV>(vertex.Value));
            }

            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(true).AddVerteces(verteces.Values);

            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                IWeightedDirectedEdge<TV, TW> directedEdge = (IWeightedDirectedEdge<TV, TW>)edge;
                TW currentFlow = directedEdge.GetAttribute<TW>(ATTR_FLOW);
                TW maxFlow = directedEdge.Weight;
                TW residualFlow = substractFlowValues(maxFlow, currentFlow);

                // Add forward edge with residual flow value as capacity
                if (residualFlow.CompareTo(zeroValue) > 0)
                {
                    Edge<TV, TW> forwardEdge = new Edge<TV, TW>(verteces[directedEdge.OriginVertex.Value],
                        verteces[directedEdge.TargetVertex.Value],
                        residualFlow);
                    forwardEdge.SetAttribute(ATTR_DIRECTION, EdgeDirection.Forward);
                    builder.AddEdge(forwardEdge);
                }

                // Add backward edge with current flow as capacity
                if (currentFlow.CompareTo(zeroValue) > 0)
                {
                    Edge<TV, TW> backwardEdge = new Edge<TV, TW>(verteces[directedEdge.TargetVertex.Value],
                        verteces[directedEdge.OriginVertex.Value], currentFlow);
                    backwardEdge.SetAttribute(ATTR_DIRECTION, EdgeDirection.Backward);
                    builder.AddEdge(backwardEdge);
                }
            }

            return builder.Build();
        }

        private static bool TryFindPath<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target,
            out List<IWeightedEdge<TV, TW>> path) where TV : IEquatable<TV> where TW : IComparable
        {
            Queue<IVertex<TV>> verteces = new Queue<IVertex<TV>>();
            HashSet<IVertex<TV>> visited = new HashSet<IVertex<TV>>();
            Dictionary<IVertex<TV>, IVertex<TV>> predecessors = new Dictionary<IVertex<TV>, IVertex<TV>>();
            path = null;

            // Starting vertex
            verteces.Enqueue(source);
            visited.Add(source);

            // Traverse graph
            while (verteces.Any())
            {
                // Get next vertex to visit
                IVertex<TV> vertex = verteces.Dequeue();

                // Check whether the target vertex was reached
                if (vertex == target)
                {
                    path = new List<IWeightedEdge<TV, TW>>();
                    while (vertex != source)
                    {
                        IVertex<TV> predecessor = predecessors[vertex];
                        path.Add(graph.GetEdgeBetweenVerteces(predecessor, vertex));
                        vertex = predecessor;
                    }
                    path.Reverse();
                    return true;
                }

                // Enqueue adjacent verteces that have not been visited yet
                foreach (IVertex<TV> adjacentVertex in graph.GetAdjacentVerteces(vertex))
                {
                    if (!visited.Contains(adjacentVertex))
                    {
                        verteces.Enqueue(adjacentVertex);
                        visited.Add(adjacentVertex);
                        predecessors.Add(adjacentVertex, vertex);
                    }
                }
            }

            // No path found!
            return false;
        }

        public enum EdgeDirection
        {
            Forward,
            Backward
        }
    }
}
