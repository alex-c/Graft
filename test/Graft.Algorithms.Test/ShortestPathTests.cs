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
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege1.txt", Parser);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);
            TestShortestPathsAlgorithms(graph, source, target, 6);
        }

        [TestMethod]
        public void TestShortestPath2()
        {
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege2.txt", Parser);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);
            try
            {
                TestShortestPathsAlgorithm(graph, source, target, 2, ShortestPathAlgorithm.Dijkstra);
            }
            catch (NegativeEdgeWeightException) { /* Expected exception */ }
        }

        [TestMethod]
        public void TestShortestPath3()
        {
            Graph<int, double> graph = Factory.CreateGraphFromFile("./graphs/paths/Wege3.txt", Parser);
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 0);
            TestShortestPathsAlgorithms(graph, source, target, 6); // TODO: graph contains a negative cycle...
        }

        [TestMethod]
        public void TestShortestPathForBigGraph()
        {
            // Undirected graph
            Graph<int, double> graphUnDirected = Factory.CreateGraphFromFile("./graphs/weighted/G_1_2.txt", Parser);
            IVertex<int> source = graphUnDirected.GetFirstMatchingVertex(v => v.Value == 0);
            IVertex<int> target = graphUnDirected.GetFirstMatchingVertex(v => v.Value == 1);
            TestShortestPathsAlgorithms(graphUnDirected, source, target, 2.36796);

            // Directed graph
            Graph<int, double> graphDirected = Factory.CreateGraphFromFile("./graphs/weighted/G_1_2.txt", Parser, true);
            source = graphDirected.GetFirstMatchingVertex(v => v.Value == 0);
            target = graphDirected.GetFirstMatchingVertex(v => v.Value == 1);
            TestShortestPathsAlgorithms(graphDirected, source, target, 5.54417);
        }

        private void TestShortestPathsAlgorithms(IWeightedGraph<int, double> graph, IVertex<int> source, IVertex<int> target, double pathCosts)
        {
            TestShortestPathsAlgorithm(graph, source, target, pathCosts, ShortestPathAlgorithm.Dijkstra);
        }

        private void TestShortestPathsAlgorithm(IWeightedGraph<int, double> graph, IVertex<int> source, IVertex<int> target, double pathCosts, ShortestPathAlgorithm algorithm)
        {
            IWeightedGraph<int, double> shortestPath = null;
            switch (algorithm)
            {
                case ShortestPathAlgorithm.Dijkstra:
                    shortestPath = Dijkstra.FindShortestPath(graph, source, target, 0.0);
                    break;
                default:
                    throw new NotSupportedException($"Testing shortest path for the {algorithm} algorithm is currently not supported.");
            }
            Assert.AreEqual(pathCosts, shortestPath.GetAllEdges().Sum(e => e.Weight));
        }
    }

    internal enum ShortestPathAlgorithm
    {
        Dijkstra
    }
}
