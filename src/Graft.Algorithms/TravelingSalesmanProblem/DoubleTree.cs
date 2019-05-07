using Graft.Algorithms.MinimumSpanningTree;
using Graft.Algorithms.Search;
using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.TravelingSalesmanProblem
{
    public static class DoubleTree
    {
        public static IWeightedGraph<TV, TW> FindTour<TV, TW>(IWeightedGraph<TV, TW> graph) where TV : IEquatable<TV>
        {
            GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>();
            builder.AddVerteces(graph.GetAllVerteces().Select(v => v.Value));

            // Find minimum spanning tree
            IWeightedGraph<TV, TW> msp = Kruskal.FindMinimumSpanningTree(graph);

            // Get order of verteces in the MSP
            HashSet<IVertex<TV>> vertexOrder = new HashSet<IVertex<TV>>();
            DepthFirstSearch.Traverse(msp, v => vertexOrder.Add(v));

            // Connect verteces in order
            IVertex<TV> lastVertex = null;
            foreach (IVertex<TV> vertex in vertexOrder)
            {
                if (lastVertex != null)
                {
                    try
                    {
                        builder.AddEdge(lastVertex.Value, vertex.Value, graph.GetEdgeBetweenVerteces(lastVertex, vertex).Weight);
                    }
                    catch (VertecesNotConnectedException<TV> exception)
                    {
                        throw new GraphNotCompleteException("The graph is not complete.", exception);
                    }
                }
                lastVertex = vertex;
            }
            // TODO: merge traversal of MSP and building of path for 1 less loop

            // Done!
            return builder.Build();
        }
    }
}
