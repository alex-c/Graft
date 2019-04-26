using System;
using System.Collections.Generic;
using System.Text;

namespace Graft.DataStructures
{
    public class FibonacciHeap<T> where T : IComparable
    {
        private FibonacciHeapNode<T> Root { get; set; }

        public bool Empty
        {
            get
            {
                return Root == null;
            }
        }
        
        public T Minimum
        {
            get
            {
                if (Empty)
                {
                    throw new InvalidOperationException("Heap is empty.");
                }
                return Root.Value;
            }
        }

        public T ExtractMinimum()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            // Step 1 - remove minimum element
            FibonacciHeapNode<T> minimumNode = Root;
            Root = minimumNode.LeftSibling ?? minimumNode.RightSibling;
            if (minimumNode.LeftSibling != null)
            {
                minimumNode.LeftSibling = minimumNode.RightSibling;
            }
            if (minimumNode.RightSibling != null)
            {
                minimumNode.RightSibling = minimumNode.LeftSibling;
            }
            if (minimumNode.Children != null)
            {
                if (Root == null)
                {
                    Root = minimumNode.Children;
                }
                else
                {
                    Merge(Root, minimumNode.Children);
                }
            }

            // Step 2 - update minimum element if needed
            if (Root != null)
            {
                Root.UpdateMinimumAmongChildren();
            }

            // Step 3 - consolidate trees

            // Done, return minimum
            return minimumNode.Value;
        }

        private void Merge(FibonacciHeapNode<T> heap1, FibonacciHeapNode<T> heap2)
        {
            heap1.RightSibling = heap2;
            heap2.LeftSibling = heap1;
        }
    }

    internal class FibonacciHeapNode<T> where T : IComparable
    {
        public T Value { get; }

        public T Minimum
        {
            get
            {
                return MinimumAmongSibglings.Value;
            }
        }

        public FibonacciHeapNode<T> Children { get; set; }

        public FibonacciHeapNode<T> LeftSibling { get; set; }

        public FibonacciHeapNode<T> RightSibling { get; set; }

        public FibonacciHeapNode<T> MinimumAmongSibglings { get; set; }

        public bool HasLostAChild { get; set; }

        public FibonacciHeapNode(T value)
        {
            Value = value;
        }

        public FibonacciHeapNode<T> GetLeftmostSibling()
        {
            if (LeftSibling == null)
            {
                return this;
            }
            else
            {
                return LeftSibling.GetLeftmostSibling();
            }
        }

        public FibonacciHeapNode<T> GetRightmostSibling()
        {
            if (RightSibling == null)
            {
                return this;
            }
            else
            {
                return RightSibling.GetRightmostSibling();
            }
        }

        public void UpdateMinimumAmongChildren()
        {
            FibonacciHeapNode<T> minimumNode = this;

            // Find minimum
            FibonacciHeapNode<T> leftIterator = LeftSibling;
            FibonacciHeapNode<T> rightIterator = RightSibling;

            while (leftIterator != null)
            {
                if (leftIterator.Value.CompareTo(minimumNode.Value) < 0)
                {
                    minimumNode = leftIterator;
                }
                leftIterator = leftIterator.LeftSibling;
            }

            while (rightIterator != null)
            {
                if (rightIterator.Value.CompareTo(minimumNode.Value) < 0)
                {
                    minimumNode = rightIterator;
                }
                rightIterator = rightIterator.RightSibling;
            }

            leftIterator = LeftSibling;
            rightIterator = RightSibling;
        }
    }
}
