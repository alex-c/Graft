using Graft.Algorithms.TravelingSalesmanProblem;
using Graft.Default;
using Graft.Default.File;
using Graft.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class TravelingSalesmanProblem
    {
        private GraphFactory<int, double> Factory { get; }

        public TravelingSalesmanProblem()
        {
            Factory = new GraphFactory<int, double>();
        }

        [TestMethod]
        [ExpectedException(typeof(GraphNotCompleteException))]
        public void TestNearestNerighborFailOnIncompleteGraph()
        {
            Graph<int, double> graph = new GraphBuilder<int, double>()
                .AddVerteces(5, v => v)
                .AddEdge(0, 2)
                .AddEdge(2, 1)
                .AddEdge(2, 3)
                .AddEdge(2, 4)
                .Build();

            NearestNeighbor.FindTour(graph);
        }

        [TestMethod]
        [ExpectedException(typeof(GraphNotCompleteException))]
        public void TestDoubleTreeFailOnIncompleteGraph()
        {
            Graph<int, double> graph = new GraphBuilder<int, double>()
                .AddVerteces(5, v => v)
                .AddEdge(0, 2)
                .AddEdge(2, 1)
                .AddEdge(2, 3)
                .AddEdge(2, 4)
                .Build();

            NearestNeighbor.FindTour(graph);
        }

        [TestMethod]
        public void TestTinyNearestNeighbor()
        {
            // Load complete graph with 10 verteces
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/complete/K_10.txt", new DefaultGraphTextLineParser());

            // Find route with double tree algorithm
            IWeightedGraph<int, double> route = NearestNeighbor.FindTour(graph);

            // Check result
            Assert.AreEqual(10, route.VertexCount);
            Assert.AreEqual(9, route.GetAllEdges().Count());
        }

        [TestMethod]
        public void TestTinyDoubleTree()
        {
            // Load complete graph with 10 verteces
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/complete/K_10.txt", new DefaultGraphTextLineParser());
            
            // Find route with double tree algorithm
            IWeightedGraph<int, double> route = DoubleTree.FindTour(graph);

            // Check result
            Assert.AreEqual(10, route.VertexCount);
            Assert.AreEqual(9, route.GetAllEdges().Count());
        }
    }
}
