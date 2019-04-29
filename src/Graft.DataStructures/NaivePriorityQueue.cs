using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.DataStructures
{
    public class NaivePriorityQueue<TE, TP> : IPriorityQueue<TE, TP> where TP : IComparable
    {
        private Dictionary<TE, TP> Elements { get; }

        public bool Empty => throw new NotImplementedException();

        public NaivePriorityQueue()
        {
            Elements = new Dictionary<TE, TP>();
        }

        public TE Dequeue()
        {
            return Elements.OrderBy(e => e.Value).FirstOrDefault().Key;
        }

        public void Enqueue(TE element, TP priority)
        {
            Elements.Add(element, priority);
        }

        public void UpdatePriority(TE element, TP priority)
        {
            Elements[element] = priority;
        }
    }
}
