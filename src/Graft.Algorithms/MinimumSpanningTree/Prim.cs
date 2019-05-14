using Graft.DataStructures;
using Graft.Default;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// An implementation of Prim's algorithm.
    /// </summary>
    public static class Prim
    {
        /// <summary>
        /// Builds the minimum spanning tree of a given graph.
        /// </summary>
        /// <typeparam name="TV">Type of the graph vertex values.</typeparam>
        /// <typeparam name="TW">Type of the graph edge weights.</typeparam>
        /// <param name="graph">The graph got which to build the minimum spanning tree.</param>
        /// <returns>Returns the minimum spanning tree of the given graph.</returns>
        public static IWeightedGraph<TV, TW> FindMinimumSpanningTree<TV, TW>(IWeightedGraph<TV, TW> graph, TW min, TW max) 
            where TV : IEquatable<TV>
            where TW : IComparable
        {
            // Builder used to incrementally build the target minimum spanning tree
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();

            // Get all verteces and edges of graph
            IEnumerable<IVertex<TV>> verteces = graph.GetAllVerteces();
            IEnumerable<IWeightedEdge<TV, TW>> edges = graph.GetAllEdges();

            // TODO: replace with more performant priority queue implementation
            // Track minimum known cost to connect to target vertex
            IPriorityQueue<TV, TW> vertexCosts = new NaivePriorityQueue<TV, TW>();

            // Track target vertex and edge connecting to target vertex with minimum known cost
            Dictionary<TV, IVertex<TV>> vertexValue = new Dictionary<TV, IVertex<TV>>();
            Dictionary<TV, IWeightedEdge<TV, TW>> vertexEdges = new Dictionary<TV, IWeightedEdge<TV, TW>>();

            // Initialize vertex costs with all costs set to max
            foreach (IVertex<TV> vertex in verteces)
            {
                vertexCosts.Enqueue(vertex.Value, max);
                vertexValue.Add(vertex.Value, vertex);
                vertexEdges.Add(vertex.Value, null);
            }

            // Set starting vertex
            vertexCosts.UpdatePriority(verteces.First().Value, min);

            // While there are verteces left to add, select vertex with smallest cost
            while (!vertexCosts.Empty)
            {
                // Get and remove vertex with smallest cost
                TV minCost = vertexCosts.Dequeue().Item1;

                // For easier handling, get vertex and edge
                IVertex<TV> vertex = vertexValue[minCost];
                IWeightedEdge<TV, TW> edge = vertexEdges[minCost];

                // Add vertex and edge to target MST
                builder.AddVertex(vertex.Value);
                if (edge != null)
                {
                    builder.AddEdge(vertex.Value, edge.ConnectedVertex(vertex.Value).Value, edge.Weight);
                }

                // Update vertex cost for adjacent verteces and store edges
                foreach (IWeightedEdge<TV, TW> connectedEdge in graph.GetEdgesOfVertex(vertex))
                {
                    // Ignore edges leading to verteces already added to the MST
                    IVertex<TV> targetVertex = connectedEdge.ConnectedVertex(vertex);
                    if (vertexCosts.Contains(targetVertex.Value) &&
                        connectedEdge.Weight.CompareTo(vertexCosts.GetPriorityOf(targetVertex.Value)) < 0)
                    {
                        vertexCosts.UpdatePriority(targetVertex.Value, connectedEdge.Weight);
                        vertexEdges[targetVertex.Value] = connectedEdge;
                    }
                }
            }

            // All verteces added to MST - done!
            return builder.Build();
        }
    }
}
