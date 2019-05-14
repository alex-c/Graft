using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.DataStructures
{
    public class NaivePriorityQueue<TE, TP> : IPriorityQueue<TE, TP> where TP : IComparable
    {
        private Dictionary<TE, TP> Elements { get; }

        public bool Empty => Elements.Count == 0;

        public int Count => Elements.Count;

        public NaivePriorityQueue()
        {
            Elements = new Dictionary<TE, TP>();
        }

        public bool Contains(TE element)
        {
            return Elements.Keys.Contains(element);
        }

        public TP GetPriorityOf(TE element)
        {
            if (Contains(element))
            {
                return Elements[element];
            }
            else
            {
                throw new KeyNotFoundException("Element not contained in queue!");
            }
        }

        public (TE, TP) Dequeue()
        {
            TE minElement = Elements.First().Key;
            TP minPriority = Elements.First().Value;
            foreach (KeyValuePair<TE, TP> element in Elements)
            {
                if (element.Value.CompareTo(minPriority) == -1)
                {
                    minElement = element.Key;
                    minPriority = element.Value;
                }
            }
            Elements.Remove(minElement);
            return (minElement, minPriority);
        }

        public void Enqueue(TE element, TP priority)
        {
            Elements.Add(element, priority);
        }

        // Should add the element if not present
        public void UpdatePriority(TE element, TP priority)
        {
            Elements[element] = priority;
        }
    }
}
