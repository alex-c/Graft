using Graft.Default;
using Graft.Default.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graft.Algorithms.Test
{
    [TestClass]
    public class ConnectedComponentTests
    {
        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        public ConnectedComponentTests()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
        }

        [TestMethod]
        public void TestTinyGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph1.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(2, components);
        }

        [TestMethod]
        public void TestSmallGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph2.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(4, components);
        }

        [TestMethod]
        public void TestMediumGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph3.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(4, components);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_gross.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(222, components);
        }

        [TestMethod]
        public void TestReallyBigGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_ganzgross.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(9560, components);
        }

        [TestMethod]
        public void TestHugeGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_ganzganzgross.txt");
            int components = ConnectedComponents.Count(graph);

            Assert.AreEqual(306, components);
        }

        private IGraph<int> ReadGraphFromFile(string filePath)
        {
            return Factory.CreateGraphFromFile(filePath, Parser, false);
        }
    }
}
