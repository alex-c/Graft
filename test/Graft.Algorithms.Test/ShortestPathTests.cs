using Graft.Algorithms.ShortestPath;
using Graft.Default;
using Graft.Default.File;
using Graft.Exceptions;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class ShortestPathTests
    {
        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        public ShortestPathTests()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
        }

        [TestMethod]
        public void TestShortestPath1()
        {
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege1.txt", Parser, true);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);
            TestShortestPathsAlgorithms(graph, source, target, 6);
        }

        [TestMethod]
        public void TestShortestPath2()
        {
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege2.txt", Parser, true);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);
            TestShortestPathsAlgorithm(graph, source, target, 2, ShortestPathAlgorithm.BellmanFordMoore);

            // Dijkstra fails for graphs with megative edges
            try
            {
                TestShortestPathsAlgorithm(graph, source, target, 2, ShortestPathAlgorithm.Dijkstra);
            }
            catch (NegativeEdgeWeightException) { /* Expected exception */ }
        }

        [TestMethod]
        public void TestShortestPath3()
        {
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege3.txt", Parser, true);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);

            // Dijkstra fails for graphs with megative edges
            try
            {
                TestShortestPathsAlgorithm(graph, source, target, 9000, ShortestPathAlgorithm.Dijkstra);
            }
            catch (NegativeEdgeWeightException) { /* Expected exception */ }

            // Bellman-Ford-Moore fails for graphs with megative cycles
            try
            {
                TestShortestPathsAlgorithm(graph, source, target, 9000, ShortestPathAlgorithm.BellmanFordMoore);
            }
            catch (NegativeCycleException) { /* Expected exception */ }
        }

        [TestMethod]
        public void TestShortestPathForBigGraph()
        {
            // Directed graph
            Graph<int, double> graphDirected = Factory.CreateGraphFromFile("./graphs/weighted/G_1_2.txt", Parser, true);
            IVertex<int> source = graphDirected.GetFirstMatchingVertex(v => v.Value == 0);
            IVertex<int> target = graphDirected.GetFirstMatchingVertex(v => v.Value == 1);
            TestShortestPathsAlgorithms(graphDirected, source, target, 5.54417, 0.00001);

            // Undirected graph
            Graph<int, double> graphUnDirected = Factory.CreateGraphFromFile("./graphs/weighted/G_1_2.txt", Parser);
            source = graphUnDirected.GetFirstMatchingVertex(v => v.Value == 0);
            target = graphUnDirected.GetFirstMatchingVertex(v => v.Value == 1);
            TestShortestPathsAlgorithms(graphUnDirected, source, target, 2.36796, 0.00001);
        }

        private void TestShortestPathsAlgorithms(IWeightedGraph<int, double> graph,
            IVertex<int> source,
            IVertex<int> target,
            double pathCosts,
            double precision = 0.1)
        {
            TestShortestPathsAlgorithm(graph, source, target, pathCosts, ShortestPathAlgorithm.BellmanFordMoore, precision);
            TestShortestPathsAlgorithm(graph, source, target, pathCosts, ShortestPathAlgorithm.Dijkstra, precision);
        }

        private void TestShortestPathsAlgorithm(IWeightedGraph<int, double> graph,
            IVertex<int> source,
            IVertex<int> target,
            double pathCosts,
            ShortestPathAlgorithm algorithm,
            double precision = 0.1)
        {
            IWeightedGraph<int, double> shortestPath = null;
            switch (algorithm)
            {
                case ShortestPathAlgorithm.Dijkstra:
                    shortestPath = Dijkstra.FindShortestPath(graph, source, target, 0.0, double.MaxValue, (x, y) => x + y);
                    break;
                case ShortestPathAlgorithm.BellmanFordMoore:
                    shortestPath = BellmanFordMoore.FindShortestPath(graph, source, target, 0.0, double.MaxValue, (x, y) => x + y);
                    break;
                default:
                    throw new NotSupportedException($"Testing shortest path for the {algorithm} algorithm is currently not supported.");
            }
            AssertDoublesNearlyEqual(pathCosts, shortestPath.GetAllEdges().Sum(e => e.Weight), precision);
        }

        private void AssertDoublesNearlyEqual(double expected, double actual, double precision)
        {
            if (actual < expected - precision || actual > expected + precision)
            {
                throw new AssertFailedException($"Doubles are not equal with a precision of {precision}. Expected {expected}, got {actual}.");
            }
        }
    }

    internal enum ShortestPathAlgorithm
    {
        Dijkstra,
        BellmanFordMoore
    }
}
