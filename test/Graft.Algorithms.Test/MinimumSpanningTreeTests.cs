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

            Assert.AreEqual(286.711, mspWeight);
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string filePath)
        {
            return Factory.CreateGraphFromFile(filePath, Parser, false);
        }
    }
}
