using System;

namespace Graft.DataStructures
{
    public interface IPriorityQueue<TE, TP> where TP : IComparable
    {
        bool Empty { get; }

        int Count { get; }

        bool Contains(TE element);

        TP GetPriorityOf(TE element);

        void Enqueue(TE element, TP priority);

        (TE, TP) Dequeue();

        void UpdatePriority(TE element, TP priority);
    }
}
