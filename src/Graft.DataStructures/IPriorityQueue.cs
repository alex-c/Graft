using System;
using System.Collections.Generic;
using System.Text;

namespace Graft.DataStructures
{
    public interface IPriorityQueue<TE, TP> where TP : IComparable
    {
        bool Empty { get; }

        int Count { get; }

        void Enqueue(TE element, TP priority);

        TE Dequeue();

        void UpdatePriority(TE element, TP priority);
    }
}
