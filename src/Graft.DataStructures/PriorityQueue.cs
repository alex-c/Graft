using System;

namespace Graft.DataStructures
{
    public class PriorityQueue<TE, TP> : IPriorityQueue<TE, TP> where TP : IComparable
    {
        private FibonacciHeap<TE, TP> Heap { get; }

        public bool Empty => Heap.Empty;

        public int Count => Heap.Count;

        public PriorityQueue()
        {
            Heap = new FibonacciHeap<TE, TP>();
        }

        public bool Contains(TE element)
        {
            return Heap.Contains(element);
        }

        public TP GetPriorityOf(TE element)
        {
            return Heap.GetPriorityOf(element);
        }

        public void Enqueue(TE element, TP priority)
        {
            Heap.Insert(element, priority);
        }

        public TE Dequeue()
        {
            return Heap.ExtractMinimum();
        }

        public void UpdatePriority(TE element, TP priority)
        {
            Heap.DecreaseKey(element, priority);
        }
    }
}
