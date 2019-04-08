using Graft.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graft.Tests
{
    [TestClass]
    public class DefaultVertexTests
    {
        [TestMethod]
        public void VertexInitialization()
        {
            Vertex<int> intVertex = new Vertex<int>(1);
            Assert.AreEqual(1, intVertex.Value);

            Vertex<double> doubleVertex = new Vertex<double>(3.16);
            Assert.AreEqual(3.16, doubleVertex.Value);

            Vertex<string> stringVertex = new Vertex<string>("test");
            Assert.AreEqual("test", stringVertex.Value);
        }
    }
}