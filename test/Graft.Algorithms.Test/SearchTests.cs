using Graft.Algorithms.Search;
using Graft.Default;
using Graft.Default.File;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Graft.Algorithms.Test
{
    [TestClass]
    public class SearchTests
    {
        private IGraph<int> Graph { get; }

        public SearchTests()
        {
            GraphFactory factory = new GraphFactory();
            Graph = factory.CreateGraphFromFile("./graphs/Graph1.txt", new DefaultGraphTextLineParser());
        }

        [TestMethod]
        public void TestBreadthFirstSearchWithDefaultStart()
        {
            List<int> traversalOrder = new List<int>();

            BreadthFirstSearch.Search(Graph, v => traversalOrder.Add(v.Value));

            Assert.AreEqual(7, traversalOrder.Count);
            CollectionAssert.AreEqual(new List<int> { 0, 6, 9, 13, 3, 5, 10 }, traversalOrder);
        }

        [TestMethod]
        public void TestBreadthFirstSearchWithExplicitStart()
        {
            List<int> traversalOrder = new List<int>();

            IVertex<int> startingVertex = Graph.GetFirstMatchingVertex(v => v.Value == 1);
            BreadthFirstSearch.Search(Graph, startingVertex, v => traversalOrder.Add(v.Value));

            Assert.AreEqual(8, traversalOrder.Count);
            CollectionAssert.AreEqual(new List<int> { 1, 4, 8, 14, 7, 2, 12, 11 }, traversalOrder);
        }

        [TestMethod]
        public void TestDepthFirstSearchWithDefaultStart()
        {
            List<int> traversalOrder = new List<int>();

            DepthFirstSearch.Search(Graph, v => traversalOrder.Add(v.Value));

            Assert.AreEqual(7, traversalOrder.Count);
            CollectionAssert.AreEqual(new List<int> { 0, 6, 3, 5, 10, 9, 13 }, traversalOrder);
        }

        [TestMethod]
        public void TestDepthFirstSearchWithExplicitStart()
        {
            List<int> traversalOrder = new List<int>();

            IVertex<int> startingVertex = Graph.GetFirstMatchingVertex(v => v.Value == 1);
            DepthFirstSearch.Search(Graph, startingVertex, v => traversalOrder.Add(v.Value));

            Assert.AreEqual(8, traversalOrder.Count);
            CollectionAssert.AreEqual(new List<int> { 1, 4, 7, 2, 11, 12, 8, 14 }, traversalOrder);
        }
    }
}
