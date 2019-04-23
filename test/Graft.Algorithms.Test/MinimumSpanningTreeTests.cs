using Graft.Algorithms.MinimumSpanningTree;
using Graft.Default;
using Graft.Default.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class MinimumSpanningTreeTests
    {
        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        public MinimumSpanningTreeTests()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
        }

        [TestMethod]
        public void TestTinyGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_1_2.txt");
            TestMstAlgorithms(graph, 286.711, 0.001);
        }

        [TestMethod]
        public void TestSmallGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_1_20.txt");
            TestMstAlgorithms(graph, 29.5493, 0.0001);
        }

        [TestMethod]
        public void TestMediumGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_1_200.txt");
            TestMstAlgorithms(graph, 3.0228, 0.0001);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_10_20.txt");
            TestMstAlgorithms(graph, 2775.44, 0.01);
        }

        [TestMethod]
        public void TestVeryBigGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_10_200.txt");
            TestMstAlgorithms(graph, 301.552, 0.001);
        }

        [TestMethod]
        public void TestHugeGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_100_200.txt");
            TestMstAlgorithms(graph, 27450.6, 0.1);
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string filePath)
        {
            return Factory.CreateGraphFromFile(filePath, Parser, false);
        }

        private void TestMstAlgorithms(IWeightedGraph<int, double> graph, double expectedMstWeight, double precision)
        {
            TestMstAlgorithm(graph, expectedMstWeight, precision, ALGORITHM.KRUSKAL);
            TestMstAlgorithm(graph, expectedMstWeight, precision, ALGORITHM.PRIM);
        }

        private void TestMstAlgorithm(IWeightedGraph<int, double> graph, double expectedMstWeight, double precision, ALGORITHM algorithm)
        {
            // Build minimum spanning tree with algorithm to test
            IWeightedGraph<int, double> msp;
            switch (algorithm)
            {
                case ALGORITHM.KRUSKAL:
                    msp = Kruskal.FindMinimumSpanningTree(graph);
                    break;
                case ALGORITHM.PRIM:
                    msp = Prim.FindMinimumSpanningTree(graph, 0, double.MaxValue);
                    break;
                default:
                    throw new NotImplementedException($"No minimum spanning tree test implemented for algorithm {algorithm.ToString()}.");
            }

            // Check that all verteces are included
            Assert.AreEqual(graph.VertexCount, msp.VertexCount);

            // Compute total weight
            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            // Check that the total weight is as expected
            AssertDoublesNearlyEqual(expectedMstWeight, mspWeight, precision);
        }

        private void AssertDoublesNearlyEqual(double expected, double actual, double precision)
        {
            if (actual < expected - precision || actual > expected + precision)
            {
                throw new AssertFailedException($"Doubles are not equal with a precision of {precision}. Expected {expected}, got {actual}.");
            }
        }
    }

    internal enum ALGORITHM
    {
        KRUSKAL,
        PRIM
    }
}
