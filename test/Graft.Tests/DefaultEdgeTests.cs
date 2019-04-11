using Graft.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graft.Tests
{
    [TestClass]
    public class DefaultEdgeTests
    {
        [TestMethod]
        public void EdgeDefaultInitialization()
        {
            Vertex<int> originVertex = new Vertex<int>(5);
            Vertex<int> targetVertex = new Vertex<int>(7);
            Edge<int, double> defaultEdge = new Edge<int, double>(originVertex, targetVertex);

            Assert.AreEqual(originVertex, defaultEdge.OriginVertex);
            Assert.AreEqual(targetVertex, defaultEdge.TargetVertex);
            Assert.AreEqual(0.0, defaultEdge.Weight);
        }

        [TestMethod]
        public void EdgeWeighterInitialization()
        {
            Vertex<int> originVertex = new Vertex<int>(5);
            Vertex<int> targetVertex = new Vertex<int>(7);
            Edge<int, double> weightedEdge = new Edge<int, double>(originVertex, targetVertex, false, 3.16);

            Assert.AreEqual(originVertex, weightedEdge.OriginVertex);
            Assert.AreEqual(targetVertex, weightedEdge.TargetVertex);
            Assert.AreEqual(3.16, weightedEdge.Weight);
        }

        [TestMethod]
        public void EdgeAttributeTests()
        {
            Vertex<int> originVertex = new Vertex<int>(5);
            Vertex<int> targetVertex = new Vertex<int>(7);
            Edge<int, double> edge = new Edge<int, double>(originVertex, targetVertex);

            Assert.AreEqual(false, edge.HasAttribute("test"));

            edge.SetAttribute("test", 5);

            Assert.AreEqual(true, edge.HasAttribute("test"));
            
            Assert.AreEqual(5, edge.GetAttribute("test"));
        }
    }
}
