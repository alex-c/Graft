using Graft.Algorithms.MinimumSpanningTree;
using Graft.Algorithms.Search;
using Graft.Default;
using Graft.Exceptions;
using Graft.Primitives;
using System;
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

            // Connect verteces in the order of the MSP
            IVertex<TV> lastVertex = null;
            DepthFirstSearch.Traverse(msp, currentVertex =>
            {
                if (lastVertex != null)
                {
                    try
                    {
                        builder.AddEdge(lastVertex.Value, currentVertex.Value, graph.GetEdgeBetweenVerteces(lastVertex.Value, currentVertex.Value).Weight);
                    }
                    catch (VertecesNotConnectedException<TV> exception)
                    {
                        throw new GraphNotCompleteException("The graph is not complete.", exception);
                    }
                }
                lastVertex = currentVertex;
            });

            // Add closing edge
            IVertex<TV> firstVertex = msp.GetFirstVertex();
            IWeightedEdge<TV, TW> closingEdge = graph.GetEdgeBetweenVerteces(lastVertex.Value, firstVertex.Value);
            builder.AddEdge(lastVertex.Value, firstVertex.Value, closingEdge.Weight);

            // Done!
            return builder.Build();
        }
    }
}
