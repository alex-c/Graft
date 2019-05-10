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
        public static IWeightedGraph<TV, TW> FindShortestPath<TV, TW>(IWeightedGraph<TV, TW> graph, IVertex<TV> source, IVertex<TV> target)
            where TV : IEquatable<TV>
            where TW : IComparable
        {
            HashSet<IVertex<TV>> visitedVerteces = new HashSet<IVertex<TV>>();
            IPriorityQueue<IVertex<TV>, TW> vertecesToVisit = new NaivePriorityQueue<IVertex<TV>, TW>();
            Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>> edgesToVerteces = new Dictionary<IVertex<TV>, IWeightedEdge<TV, TW>>();

            // Set starting vertex and first verteces to visit with costs
            IVertex<TV> currentVertex = source;
            foreach (IWeightedEdge<TV, TW> connectedEdge in graph.GetEdgesOfVertex(currentVertex))
            {
                IVertex<TV> connectedVertex = connectedEdge.ConnectedVertex(currentVertex);
                if (!visitedVerteces.Contains(connectedVertex))
                {
                    if (!vertecesToVisit.Contains(connectedVertex) ||
                        connectedEdge.Weight.CompareTo(vertecesToVisit.GetPriorityOf(connectedVertex)) < 0)
                    {
                        vertecesToVisit.UpdatePriority(connectedVertex, connectedEdge.Weight);
                        edgesToVerteces[connectedVertex] = connectedEdge;
                    }
                }
            }

            // Continue while there are verteces to visit
            while (!vertecesToVisit.Empty)
            {
                // Get the closest next vertex
                currentVertex = vertecesToVisit.Dequeue();

                // Check whether we reached our target
                if (currentVertex == target)
                {
                    break;
                }
                else
                {
                    visitedVerteces.Add(currentVertex);
                }

                // If not, discover the next verteces that can be visited 
                foreach (IWeightedEdge<TV, TW> edge in graph.GetEdgesOfVertex(currentVertex))
                {
                    IVertex<TV> targetVertex = edge.ConnectedVertex(currentVertex);
                    if (!visitedVerteces.Contains(targetVertex))
                    {
                        vertecesToVisit.UpdatePriority(targetVertex, edge.Weight);
                        edgesToVerteces[targetVertex] = edge;
                    }
                }
            }

            // If we reached the target vertex, build result graph
            if (currentVertex == target)
            {
                GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>().AddVertex(target.Value);
                while (currentVertex != source)
                {
                    IWeightedEdge<TV, TW> pathEdge = edgesToVerteces[currentVertex];
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
