using Graft.DataStructures;
using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Algorithms.ShortestPath
{
    public static class Dijkstra
    {
        public static IWeightedGraph<TV, TW> FindShortestPath<TV, TW>(IWeightedGraph<TV, TW> graph,
            IVertex<TV> source,
            IVertex<TV> target,
            TW zeroValue,
            TW maxValue,
            Func<TW, TW, TW> combineCosts) where TV : IEquatable<TV> where TW : IComparable
        {
            HashSet<IVertex<TV>> visitedVerteces = new HashSet<IVertex<TV>>();
            IPriorityQueue<IVertex<TV>, TW> vertecesToVisit = new NaivePriorityQueue<IVertex<TV>, TW>();
            Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>> predecessor = new Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>>();

            // Initialize queue and predecessor map
            foreach (IVertex<TV> vertex in graph.GetAllVerteces())
            {
                vertecesToVisit.Enqueue(vertex, maxValue);
                predecessor.Add(vertex, null);
            }
            vertecesToVisit.UpdatePriority(source, zeroValue);

            // Continue as long as there are verteces to visit
            IVertex<TV> currentVertex = null;
            TW currentCosts = zeroValue;
            while (!vertecesToVisit.Empty)
            {
                // Get the closest next vertex
                (currentVertex, currentCosts) = vertecesToVisit.Dequeue();

                // Check whether we reached our target
                if (currentVertex == target)
                {
                    break;
                }
                visitedVerteces.Add(currentVertex);

                // If not, discover the next verteces that can be visited 
                foreach (IWeightedEdge<TV, TW> edge in graph.GetEdgesOfVertex(currentVertex))
                {
                    if (edge.Weight.CompareTo(zeroValue) < 0)
                    {
                        throw new NegativeEdgeWeightException();
                    }
                    IVertex<TV> connectedVertex = edge.ConnectedVertex(currentVertex);
                    if (!visitedVerteces.Contains(connectedVertex))
                    {
                        TW newCosts = combineCosts(currentCosts, edge.Weight);
                        if (newCosts.CompareTo(vertecesToVisit.GetPriorityOf(connectedVertex)) < 0)
                        {
                            vertecesToVisit.UpdatePriority(connectedVertex, newCosts);
                            predecessor[connectedVertex] = edge;
                        }
                    }
                }
            }

            // If we reached the target vertex, build result graph
            if (currentVertex == target)
            {
                GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(true).AddVertex(target.Value);
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
            else
            {
                throw new VertexNotReachableException<TV>(target);
            }
        }
    }
}
