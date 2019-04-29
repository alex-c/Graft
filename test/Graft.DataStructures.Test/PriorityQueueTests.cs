using Graft.Default;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Graft.DataStructures.Test
{
    [TestClass]
    public class PriorityQueueTests 
    {
        private List<IPriorityQueue<IVertex<int>, double>> QueueImplementations { get; set; }

        [TestInitialize]
        public void SetUpQueueImplementations()
        {
            QueueImplementations = new List<IPriorityQueue<IVertex<int>, double>>();
            QueueImplementations.Add(new NaivePriorityQueue<IVertex<int>, double>());
        }

        [TestMethod]
        public void TestEmpty()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                Assert.AreEqual(true, queue.Empty);
                Assert.AreEqual(0, queue.Count);
            }
        }

        [TestMethod]
        public void TestEnqueue()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                queue.Enqueue(new Vertex<int>(1), 0.1);
                Assert.AreEqual(false, queue.Empty);
                Assert.AreEqual(1, queue.Count);
            }
        }

        [TestMethod]
        public void TestDequeue()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                queue.Enqueue(new Vertex<int>(1), 0.3);
                queue.Enqueue(new Vertex<int>(2), 0.1);
                queue.Enqueue(new Vertex<int>(3), 0.2);
                queue.Enqueue(new Vertex<int>(4), 17.7);
                queue.Enqueue(new Vertex<int>(5), 1.0);
                Assert.AreEqual(2, queue.Dequeue().Value);
                Assert.AreEqual(4, queue.Count);
                queue.Dequeue();
                queue.Dequeue();
                queue.Dequeue();
                queue.Dequeue();
                Assert.AreEqual(true, queue.Empty);
                Assert.AreEqual(0, queue.Count);
            }
        }

        [TestMethod]
        public void TestUpdatePriority()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                IVertex<int> vertexToUpdatePriorityFor = new Vertex<int>(4);
                queue.Enqueue(new Vertex<int>(1), 0.3);
                queue.Enqueue(new Vertex<int>(2), 0.1);
                queue.Enqueue(new Vertex<int>(3), 0.2);
                queue.Enqueue(vertexToUpdatePriorityFor, 17.7);
                queue.Enqueue(new Vertex<int>(5), 1.0);

                queue.UpdatePriority(vertexToUpdatePriorityFor, 0.0);
                Assert.AreEqual(vertexToUpdatePriorityFor.Value, queue.Dequeue().Value);
            }
        }
    }
}
