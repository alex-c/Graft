using Graft.Algorithms.TravelingSalesmanProblem;
using Graft.Default;
using Graft.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class TravelingSalesmanProblem
    {
        [TestMethod]
        public void TestSimpleNearestNeighbor()
        {
            GraphBuilder<int, double> builder = new GraphBuilder<int, double>();
            Graph<int, double> graph = builder.AddVerteces(5, n => n)
                .AddEdge(0, 1, 0.5)
                .AddEdge(0, 2, 0.4)
                .AddEdge(0, 3, 0.9)
                .AddEdge(1, 2, 0.1)
                .AddEdge(1, 4, 0.2)
                .AddEdge(2, 3, 0.2)
                .AddEdge(2, 4, 0.1)
                .AddEdge(3, 4, 0.3)
                .Build();

            // Start from vertex 0 - works
            IWeightedGraph<int, double> route = NearestNeighbor.FindTour(graph, graph.GetFirstMatchingVertex(v => v.Value == 1));
            Assert.AreEqual(5, route.VertexCount);
            Assert.AreEqual(4, route.GetAllEdges().Count());
            Assert.AreEqual(1.4, route.GetAllEdges().Sum(e => e.Weight));

            // Start from vertex 3 - fails because graph is not complete
            try
            {
                NearestNeighbor.FindTour(graph, graph.GetFirstMatchingVertex(v => v.Value == 3));
            }
            catch (GraphNotCompleteException) { /* expected */ }
        }
    }
}
