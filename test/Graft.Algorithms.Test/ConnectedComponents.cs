using Graft.Algorithms.Search;
using Graft.Default;
using Graft.Default.File;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace Graft.Algorithms.Test
{
    [TestClass]
    public class ConnectedComponents
    {
        private GraphFactory Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        public ConnectedComponents()
        {
            Factory = new GraphFactory();
            Parser = new DefaultGraphTextLineParser();
        }

        [TestMethod]
        public void TestTinyGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph1.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(2, components);
        }

        [TestMethod]
        public void TestSmallGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph2.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(4, components);
        }

        [TestMethod]
        public void TestMediumGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph3.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(4, components);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_gross.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(222, components);
        }

        [TestMethod]
        public void TestReallyBigGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_ganzgross.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(306, components);
        }

        [TestMethod]
        public void TestHugeGraph()
        {
            IGraph<int> graph = ReadGraphFromFile("./graphs/Graph_ganzganzgross.txt");
            int components = CountConnectedComponents(graph);

            Assert.AreEqual(9560, components);
        }

        private IGraph<int> ReadGraphFromFile(string filePath)
        {
            return Factory.CreateGraphFromFile(filePath, Parser, false);
        }

        private int CountConnectedComponents(IGraph<int> graph)
        {
            HashSet<int> visitedVerteces = new HashSet<int>();
            IVertex<int> nextComponent = graph.GetFirstVertex();
            int connectedComponents = 0;

            while (nextComponent != null)
            {
                BreadthFirstSearch.Search(graph, nextComponent, v => visitedVerteces.Add(v.Value));

                connectedComponents++;

                nextComponent = graph.GetFirstMatchingVertex(v => !visitedVerteces.Contains(v.Value));
            }

            return connectedComponents;
        }
    }
}
