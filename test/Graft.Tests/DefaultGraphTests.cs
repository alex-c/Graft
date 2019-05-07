using Graft.Default;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Graft.Tests
{
    [TestClass]
    public class DefaultGraphTests
    {
        private IWeightedGraph<int, double> TestGraph { get; }

        public DefaultGraphTests()
        {
            TestGraph = new GraphBuilder<int, double>()
                .AddVerteces(4, v => v + 1)
                .AddEdge(1, 2)
                .AddEdge(2, 3, 15.3)
                .AddEdge(3, 1)
                .AddEdge(3, 4)
                .Build();
        }

        [TestMethod]
        public void TestProperties()
        {
            Assert.AreEqual(false, TestGraph.IsDirected);
            Assert.AreEqual(4, TestGraph.VertexCount);
        }

        [TestMethod]
        public void TestVertexAccessMethods()
        {
            // Get single vertex
            Assert.AreEqual(1, TestGraph.GetFirstVertex().Value);
            Assert.AreEqual(2, TestGraph.GetFirstMatchingVertex(v => v.Value == 2).Value);

            // Get multiple verteces
            Assert.AreEqual(4, TestGraph.GetAllVerteces().Count());
            Assert.AreEqual(2, TestGraph.GetAllMatchingVerteces(v => v.Value < 3).Count());

            // Get adcacent verteces
            IVertex<int> vertex3 = TestGraph.GetFirstMatchingVertex(v => v.Value == 3);
            Assert.AreEqual(3, TestGraph.GetAdjacentVerteces(3).Count());
            Assert.AreEqual(3, TestGraph.GetAdjacentVerteces(vertex3).Count());

            // Contain checks
            Assert.AreEqual(true, TestGraph.ContainsVertex(3));
            Assert.AreEqual(true, TestGraph.ContainsVertex(vertex3));
        }

        [TestMethod]
        public void TestEdgeAccessMethods()
        {
            IVertex<int> vertex1 = TestGraph.GetFirstMatchingVertex(v => v.Value == 1);
            IVertex<int> vertex2 = TestGraph.GetFirstMatchingVertex(v => v.Value == 2);
            IVertex<int> vertex3 = TestGraph.GetFirstMatchingVertex(v => v.Value == 3);

            Assert.AreEqual(4, TestGraph.GetAllEdges().Count());
            Assert.AreEqual(3, TestGraph.GetEdgesOfVertex(vertex3).Count());
            Assert.AreEqual(15.3, TestGraph.GetEdgeBetweenVerteces(vertex2, vertex3).Weight);
        }
    }
}