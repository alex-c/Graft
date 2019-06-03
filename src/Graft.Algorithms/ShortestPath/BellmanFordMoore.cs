using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.ShortestPath
{
    public static class BellmanFordMoore
    {
        public static IWeightedGraph<TV, TW> FindShortestPath<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target,
            TW zeroValue,
            TW maxValue,
            Func<TW, TW, TW> combineCosts) where TV : IEquatable<TV> where TW : IComparable
        {
            Dictionary<IVertex<TV>, TW> distance = new Dictionary<IVertex<TV>, TW>();
            Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>> predecessor = new Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>>();

            // Initialize distance and predecessor maps
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                distance.Add(vertex, maxValue);
                predecessor.Add(vertex, null);
            }
            distance[source] = zeroValue;

            // Relax edges repeatedly
            IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetAllEdges();
            for (int i = 0; i < graph.VertexCount - 1; i++)
            {
                foreach (IWeightedEdge<TV, TW> edge in edges)
                {
                    if (graph.IsDirected)
                    {
                        try
                        {
                            IWeightedDirectedEdge<TV, TW> directedEdge = (IWeightedDirectedEdge<TV, TW>)edge;
                            RelaxEdge(distance, predecessor, directedEdge.OriginVertex, directedEdge.TargetVertex, directedEdge.Weight, directedEdge, combineCosts);
                        }
                        catch
                        {
                            throw new Exception("Failed casting edge to directed edge for directed graph.");
                        }
                    }
                    else
                    {
                        IVertex<TV> sourceVertex = edge.Verteces.First();
                        IVertex<TV> targetVertex = edge.ConnectedVertex(sourceVertex);
                        RelaxEdge(distance, predecessor, sourceVertex, targetVertex, edge.Weight, edge, combineCosts);
                        RelaxEdge(distance, predecessor, targetVertex, sourceVertex, edge.Weight, edge, combineCosts);
                    }
                }
            }

            // Check for negative - weight cycles
            foreach (IWeightedEdge<TV, TW> edge in graph.GetAllEdges())
            {
                if (graph.IsDirected)
                {
                    IWeightedDirectedEdge<TV, TW> directedEdge = (IWeightedDirectedEdge<TV, TW>)edge;
                    CheckForNegativeCycles(distance, directedEdge.OriginVertex, directedEdge.TargetVertex, directedEdge.Weight, combineCosts);
                }
                else
                {
                    IVertex<TV> sourceVertex = edge.Verteces.First();
                    IVertex<TV> targetVertex = edge.ConnectedVertex(sourceVertex);
                    CheckForNegativeCycles(distance, sourceVertex, targetVertex, edge.Weight, combineCosts);
                    CheckForNegativeCycles(distance, targetVertex, sourceVertex, edge.Weight, combineCosts);
                }
            }

            // Build and output path
            IVertex<TV> currentVertex = target;
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>().AddVertex(target.Value);
            while (currentVertex != source)
            {
                IWeightedEdge<TV, TW> pathEdge = predecessor[currentVertex];
                IVertex<TV> previousVertex = pathEdge.ConnectedVertex(currentVertex);
                builder
                    .AddVertex(previousVertex.Value)
                    .AddEdge(previousVertex.Value, currentVertex.Value, pathEdge.Weight);
                currentVertex = previousVertex;
            }
            return builder.Build();
        }

        private static void RelaxEdge<TV, TW>(Dictionary<IVertex<TV>, TW> distanceMap,
            Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>> predecessor,
            IVertex<TV> sourceVertex,
            IVertex<TV> targetVertex,
            TW weight,
            IWeightedEdge<TV, TW> edge,
            Func<TW, TW, TW> combineCosts)
            where TV : IEquatable<TV> where TW : IComparable
        {
            if (combineCosts(distanceMap[sourceVertex], weight).CompareTo(distanceMap[targetVertex]) < 0)
            {
                distanceMap[targetVertex] = combineCosts(distanceMap[sourceVertex], weight);
                predecessor[targetVertex] = edge;
            }
        }

        private static void CheckForNegativeCycles<TV, TW>(Dictionary<IVertex<TV>, TW> distanceMap,
            IVertex<TV> sourceVertex,
            IVertex<TV> targetVertex,
            TW weight,
            Func<TW, TW, TW> combineCosts)
            where TV : IEquatable<TV> where TW : IComparable
        {
            if (combineCosts(distanceMap[sourceVertex], weight).CompareTo(distanceMap[targetVertex]) < 0)
            {
                distanceMap[targetVertex] = combineCosts(distanceMap[sourceVertex], weight);
                throw new NegativeCycleException();
            }
        }
    }
}
