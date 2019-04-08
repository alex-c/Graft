using Graft.Default;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Tests
{
    [TestClass]
    public class DefaultGraphTests
    {
        IGraph<int> TestGraph { get; }

        public DefaultGraphTests()
        {
            List<Vertex<int>> verteces = new List<Vertex<int>>()
            {
                new Vertex<int>(1),
                new Vertex<int>(2),
                new Vertex<int>(3),
                new Vertex<int>(4)
            };
            Dictionary<int, List<Edge<int, double>>> edges = new Dictionary<int, List<Edge<int, double>>>
            {
                { 1, new List<Edge<int, double>>() { new Edge<int, double>(verteces[1]) } },
                { 2, new List<Edge<int, double>>() { new Edge<int, double>(verteces[2]) } },
                { 3, new List<Edge<int, double>>() { new Edge<int, double>(verteces[0]), new Edge<int, double>(verteces[3]) } }
            };
            TestGraph = new Graph<int, double>(verteces, edges);
        }

        [TestMethod]
        public void TestStuff()
        {
            Assert.AreEqual(false, TestGraph.IsDirected);
            Assert.AreEqual(1, TestGraph.GetFirstVertex().Value);
            Assert.AreEqual(2, TestGraph.GetFirstMatchingVertex(v => v.Value == 2).Value);
            Assert.AreEqual(4, TestGraph.GetVerteces().Count());
            Assert.AreEqual(2, TestGraph.GetAdjacentVerteces(3).Count());
        }
    }
}