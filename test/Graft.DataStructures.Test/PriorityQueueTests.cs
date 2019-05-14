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
            QueueImplementations = new List<IPriorityQueue<IVertex<int>, double>>
            {
                new NaivePriorityQueue<IVertex<int>, double>(),
                new PriorityQueue<IVertex<int>, double>()
            };
        }

        [TestMethod]
        public void TestEmpty()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                Assert.AreEqual(true, queue.Empty);
                Assert.AreEqual(0, queue.Count);
                Assert.AreEqual(false, queue.Contains(new Vertex<int>(0)));
            }
        }

        [TestMethod]
        public void TestEnqueue()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                IVertex<int> vertex = new Vertex<int>(1);
                queue.Enqueue(vertex, 0.1);
                Assert.AreEqual(false, queue.Empty);
                Assert.AreEqual(1, queue.Count);
                Assert.AreEqual(true, queue.Contains(vertex));
            }
        }

        [TestMethod]
        public void TestDequeue()
        {
            foreach (IPriorityQueue<IVertex<int>, double> queue in QueueImplementations)
            {
                // Populate queue
                IVertex<int> vertexToDequeue = new Vertex<int>(2);
                queue.Enqueue(new Vertex<int>(1), 0.3);
                queue.Enqueue(vertexToDequeue, 0.1);
                queue.Enqueue(new Vertex<int>(3), 0.2);
                queue.Enqueue(new Vertex<int>(4), 17.7);
                queue.Enqueue(new Vertex<int>(5), 1.0);

                // Dequeue and check first element
                Assert.AreEqual(vertexToDequeue.Value, queue.Dequeue().Item1.Value);
                Assert.AreEqual(4, queue.Count);
                Assert.AreEqual(false, queue.Contains(vertexToDequeue));

                // Dequeue other elements
                Assert.AreEqual(3, queue.Dequeue().Item1.Value);
                Assert.AreEqual(1, queue.Dequeue().Item1.Value);
                Assert.AreEqual(5, queue.Dequeue().Item1.Value);
                Assert.AreEqual(4, queue.Dequeue().Item1.Value);

                // Queue empty
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
                Assert.AreEqual(vertexToUpdatePriorityFor.Value, queue.Dequeue().Item1.Value);
            }
        }
    }
}
