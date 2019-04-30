using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Graft.DataStructures.Test
{
    [TestClass]
    public class FibonacciHeapTests
    {
        [TestMethod]
        public void TestSimpleProperties()
        {
            FibonacciHeap<int, double> heap = new FibonacciHeap<int, double>();

            // Heap empty
            Assert.AreEqual(true, heap.Empty);
            Assert.AreEqual(0, heap.Count);
            try
            {
                int min = heap.Minimum;
            } catch (InvalidOperationException) { }

            // Heap not empty
            heap.Insert(0, 0.5);
            Assert.AreEqual(false, heap.Empty);
            Assert.AreEqual(1, heap.Count);
            Assert.AreEqual(0, heap.Minimum);
        }

        [TestMethod]
        public void TestMinimum()
        {
            FibonacciHeap<int, double> heap = new FibonacciHeap<int, double>();
            heap.Insert(0, 0.9);
            heap.Insert(1, 17.9);
            heap.Insert(2, 0.333);
            heap.Insert(3, 2.7);
            heap.Insert(4, 42);

            Assert.AreEqual(2, heap.Minimum);
        }

        [TestMethod]
        public void TestExtractMinimum()
        {
            FibonacciHeap<int, double> heap = new FibonacciHeap<int, double>();
            heap.Insert(0, 0.9);
            heap.Insert(1, 17.9);
            heap.Insert(2, 0.333);
            heap.Insert(3, 2.7);
            heap.Insert(4, 42);

            Assert.AreEqual(2, heap.ExtractMinimum());
            Assert.AreEqual(4, heap.Count);
            Assert.AreEqual(0, heap.Minimum);
        }

        [TestMethod]
        public void TestDecreaseKey()
        {
            // Build heap
            FibonacciHeap<int, double> heap = new FibonacciHeap<int, double>();
            heap.Insert(0, 0.9);
            heap.Insert(1, 17.9);
            heap.Insert(2, 0.333);
            heap.Insert(3, 2.7);
            heap.Insert(4, 42);

            // Force tree consolidation by removing one node
            heap.ExtractMinimum();

            // Decrease key and check result
            heap.DecreaseKey(3, 0.5);
            Assert.AreEqual(3, heap.Minimum);
            Assert.AreEqual(4, heap.Count);

            // Extract min element again
            heap.ExtractMinimum();
            Assert.AreEqual(0, heap.Minimum);
        }

        /// <summary>
        /// Test the fibonacci heap by simulating Prim's algorithm to find the minimum spanning tree on the following graph:
        /// - 5 verteces 0-4
        /// - weighted edges:
        ///     0-1: 0.2
        ///     0-2: 0.1
        ///     0-3: 0.3
        ///     1-2: 0.1
        ///     1-3: 0.2
        ///     1-4: 1.0
        /// </summary>
        [TestMethod]
        public void TestFibonacciHeap()
        {
            FibonacciHeap<int, double> heap = new FibonacciHeap<int, double>();
            heap.Insert(0, double.MaxValue);
            heap.Insert(1, double.MaxValue);
            heap.Insert(2, double.MaxValue);
            heap.Insert(3, double.MaxValue);
            heap.Insert(4, double.MaxValue);

            // Starting point: vertex 0
            heap.DecreaseKey(0, 0);
            Assert.AreEqual(0, heap.ExtractMinimum());

            // Update edge costs from vertex 0
            heap.DecreaseKey(1, 0.2);
            heap.DecreaseKey(2, 0.1);
            heap.DecreaseKey(3, 0.3);

            // Remove vertex 2
            Assert.AreEqual(2, heap.ExtractMinimum());

            // Update edge costs from vertex 2
            heap.DecreaseKey(1, 0.1);

            // Remove vertex 1
            Assert.AreEqual(1, heap.ExtractMinimum());

            // Update edge costs from vertex 1
            heap.DecreaseKey(3, 0.2);
            heap.DecreaseKey(4, 1.0);

            // Remove last verteces
            Assert.AreEqual(3, heap.ExtractMinimum());
            Assert.AreEqual(4, heap.ExtractMinimum());
            Assert.AreEqual(true, heap.Empty);
        }
    }
}
