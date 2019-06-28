using Graft.Algorithms.MaxFlow;
using Graft.Default;
using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Algorithms.MaxMatchings
{
    public static class BipartiteMatching
    {
        public static IEnumerable<IEdge<TV>> FindMaxMatching<TV>(IGraph<TV> graph,
            ISet<IVertex<TV>> set1,
            ISet<IVertex<TV>> set2,
            TV superSourceValue,
            TV superTargetValue) where TV : IEquatable<TV>
        {
            // Build directed graph with super nodes and capacity 1
            IWeightedGraph<TV, int> graphWithSuperNodes = BuildGraphWithSuperNodesAndCapacity(graph, superSourceValue, superTargetValue, set1, set2, 1);
            IVertex<TV> sourceNode = graphWithSuperNodes.GetFirstMatchingVertex(v => v.Value.Equals(superSourceValue));
            IVertex<TV> targetNode = graphWithSuperNodes.GetFirstMatchingVertex(v => v.Value.Equals(superTargetValue));

            // Compute max flow in graph with super nodes
            IWeightedGraph<TV, int> maxFlow = EdmondsKarp.FindMaxFlow(graphWithSuperNodes,
                sourceNode,
                targetNode,
                (x, y) => x + y,
                (x, y) => x - y,
                0);

            // Add edges with flow to matching
            List<IEdge<TV>> matchings = new List<IEdge<TV>>();
            foreach (IVertex<TV> vertex in set1)
            {
                foreach (IEdge<TV> edge in graph.GetEdgesOfVertex(vertex))
                {
                    if (maxFlow.GetEdgeBetweenVerteces(vertex.Value, edge.ConnectedVertex(vertex).Value).Weight == 1)
                    {
                        matchings.Add(edge);
                    }
                }
            }

            // Done - return matching!
            return matchings;
        }

        private static IWeightedGraph<TV, int> BuildGraphWithSuperNodesAndCapacity<TV>(IGraph<TV> graph,
            TV superSourceValue,
            TV superTargetValue,
            ISet<IVertex<TV>> sources,
            ISet<IVertex<TV>> targets,
            int capacity)
            where TV : IEquatable<TV>
        {
            // Clone graph verteces
            GraphBuilder<TV, int> builder = new GraphBuilder<TV, int>(true);
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                builder.AddVertex(vertex.Value);
            }

            // Add directed edges from set1 verteces to set2 verteces
            foreach (IVertex<TV> source in sources)
            {
                foreach (IEdge<TV> edge in graph.GetEdgesOfVertex(source))
                {
                    builder.AddEdge(source.Value, edge.ConnectedVertex(source).Value, capacity);
                }
            }

            // Add super source and target
            builder.AddVertex(superSourceValue);
            builder.AddVertex(superTargetValue);

            // Add edges from super source and to super target
            foreach (IVertex<TV> source in sources)
            {
                builder.AddEdge(superSourceValue, source.Value, capacity);
            }
            foreach (IVertex<TV> target in targets)
            {
                builder.AddEdge(target.Value, superTargetValue, capacity);
            }

            // Build and return result graph
            return builder.Build();
        }
    }
}
