using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.TravelingSalesmanProblem
{
    public static class BruteForce
    {
        public static IWeightedGraph<TV, TW> FindOptimalTour<TV, TW>(IWeightedGraph<TV, TW> graph,
            TW zeroValue,
            TW maxValue,
            Func<TW, TW, TW> combineCosts) where TV : IEquatable<TV> where TW : IComparable
        {
            HashSet<IWeightedEdge<TV, TW>> minTour = new HashSet<IWeightedEdge<TV, TW>>();
            TW minCosts = maxValue;

            // Try all tours to find the optimal one
            RecursivelyTryAllTours(graph,
                graph.GetFirstVertex(),
                graph.GetFirstVertex(),
                zeroValue,
                new HashSet<IVertex<TV>>(),
                new HashSet<IWeightedEdge<TV, TW>>(),
                ref minTour,
                ref minCosts,
                combineCosts);

            // Build and return tour graph
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>()
                .AddVerteces(graph.GetAllVerteces().Select(v => v.Value));
            foreach (var edge in minTour)
            {
                IVertex<TV> edgeSource = edge.Verteces.First();
                builder.AddEdge(edgeSource.Value, edge.ConnectedVertex(edgeSource).Value, edge.Weight);
            }
            return builder.Build();
        }

        private static void RecursivelyTryAllTours<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> currentVertex,
            IVertex<TV> startingVertex,
            TW currentCosts,
            HashSet<IVertex<TV>> visitedVerteces,
            HashSet<IWeightedEdge<TV, TW>> usedEdges,
            ref HashSet<IWeightedEdge<TV, TW>> minTour,
            ref TW minCosts,
            Func<TW, TW, TW> combineCosts) where TV : IEquatable<TV> where TW : IComparable
        {
            // Check whether we can complete a tour
            if (usedEdges.Count == graph.VertexCount - 1)
            {
                try
                {
                    // Add closing edge to complete tour
                    IWeightedEdge<TV, TW> closingEdge = graph.GetEdgeBetweenVerteces(currentVertex, startingVertex);
                    usedEdges.Add(closingEdge);

                    // Check tour costs and compare to current min tour/costs
                    TW tourCosts = combineCosts(currentCosts, closingEdge.Weight);
                    if (tourCosts.CompareTo(minCosts) < 0)
                    {
                        minTour = new HashSet<IWeightedEdge<TV, TW>>(usedEdges);
                        minCosts = tourCosts;
                    }

                    // Pop closing edge
                    usedEdges.Remove(closingEdge);
                }
                catch (VertecesNotConnectedException<TV> exception)
                {
                    throw new GraphNotCompleteException("The graph is not complete!", exception);
                }
            }
            // Continue building tours using connected verteces
            else
            {
                visitedVerteces.Add(currentVertex);

                // Check all connected verteces
                foreach (IWeightedEdge<TV, TW> edge in graph.GetEdgesOfVertex(currentVertex))
                {
                    IVertex<TV> targetVertex = edge.ConnectedVertex(currentVertex);
                    if (!visitedVerteces.Contains(targetVertex))
                    {
                        usedEdges.Add(edge);

                        // Comput new tour costs and branch and bound
                        TW newCosts = combineCosts(currentCosts, edge.Weight);
                        if (newCosts.CompareTo(minCosts) < 0)
                        {
                            RecursivelyTryAllTours(graph, targetVertex, startingVertex, newCosts, visitedVerteces, usedEdges, ref minTour, ref minCosts, combineCosts);
                        }

                        // Pop last edge on the way up
                        usedEdges.Remove(edge);
                    }
                }

                // Pop last vertex on the way up
                visitedVerteces.Remove(currentVertex);
            }
        }
    }
}
