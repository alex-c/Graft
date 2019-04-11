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
            Dictionary<int, HashSet<Edge<int, double>>> edges = new Dictionary<int, HashSet<Edge<int, double>>>();
            Dictionary<int, Vertex<int>> vertexMap = new Dictionary<int, Vertex<int>>();
            for (int i = 1; i < 5; i++)
            {
                vertexMap.Add(i, new Vertex<int>(i));
                edges.Add(i, new HashSet<Edge<int, double>>());
            }
            HashSet<Vertex<int>> verteces = new HashSet<Vertex<int>>(vertexMap.Values);
            edges[1].Add(new Edge<int, double>(vertexMap[1], vertexMap[2]));
            edges[2].Add(new Edge<int, double>(vertexMap[2], vertexMap[3]));
            edges[3].Add(new Edge<int, double>(vertexMap[3], vertexMap[1]));
            edges[3].Add(new Edge<int, double>(vertexMap[3], vertexMap[4]));
            TestGraph = new Graph<int, double>(verteces, edges);
        }

        [TestMethod]
        public void TestStuff()
        {
            Assert.AreEqual(false, TestGraph.IsDirected);
            Assert.AreEqual(1, TestGraph.GetFirstVertex().Value);
            Assert.AreEqual(2, TestGraph.GetFirstMatchingVertex(v => v.Value == 2).Value);
            Assert.AreEqual(4, TestGraph.GetAllVerteces().Count());
            Assert.AreEqual(2, TestGraph.GetAdjacentVerteces(3).Count());
        }
    }
}