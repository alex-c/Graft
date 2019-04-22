using Graft.Algorithms.MinimumSpanningTree;
using Graft.Default;
using Graft.Default.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(286.711, mspWeight, 0.001);
        }

        [TestMethod]
        public void TestSmallGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_1_20.txt");

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(29.5493, mspWeight, 0.0001);
        }

        [TestMethod]
        public void TestMediumGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_1_200.txt");

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(3.0228, mspWeight, 0.0001);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_10_20.txt");

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(2775.44, mspWeight, 0.01);
        }

        [TestMethod]
        public void TestVeryBigGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_10_200.txt");

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(301.552, mspWeight, 0.001);
        }

        [TestMethod]
        public void TestVeryHugeGraph()
        {
            IWeightedGraph<int, double> graph = ReadGraphFromFile("./graphs/weighted/G_100_200.txt");

            IWeightedGraph<int, double> msp = Kruskal.FindMinimumSpanningTree(graph);

            double mspWeight = msp.GetAllEdges().Sum(e => e.Weight);

            AssertDoublesNearlyEqual(27450.6, mspWeight, 0.1);
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string filePath)
        {
            return Factory.CreateGraphFromFile(filePath, Parser, false);
        }

        private void AssertDoublesNearlyEqual(double expected, double actual, double precision)
        {
            if (actual < expected - precision || actual > expected + precision)
            {
                throw new AssertFailedException($"Doubles are not equal with a precision of {precision}. Expected {expected}, got {actual}.");
            }
        }
    }
}
