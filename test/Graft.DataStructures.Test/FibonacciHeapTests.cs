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
    }
}
