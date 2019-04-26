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

        public void Insert(T value)
        {
            var newNode = new FibonacciHeapNode<T>(value);
            if (Empty)
            {
                Root = newNode;
            }
            else
            {
                Append(Root, newNode);
                if (newNode.IsSmallerThan(Root))
                {
                    Root = newNode;
                }
            }
        }

        public T ExtractMinimum()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            // Step 1 - remove minimum element
            // Get minimum node and set root pointer to one of it's siblings (could be null if there are none)
            FibonacciHeapNode<T> minimumNode = Root;
            Root = minimumNode.LeftSibling ?? minimumNode.RightSibling;

            // Connect siblings, if any
            if (minimumNode.LeftSibling != null)
            {
                minimumNode.LeftSibling.RightSibling = minimumNode.RightSibling;
            }
            if (minimumNode.RightSibling != null)
            {
                minimumNode.RightSibling.LeftSibling = minimumNode.LeftSibling;
            }

            // Merge children into root tree list
            if (minimumNode.Children != null)
            {
                // In case the root pointer was null, set it to a child
                if (Root == null)
                {
                    Root = minimumNode.Children;
                }

                // If not merge the children into the root tree list
                else
                {
                    Append(Root, minimumNode.Children);
                }
            }

            // Step 2 - update minimum element if needed
            if (Root != null)
            {
                Root = Root.GetMinimumNodeAmongSibglings();
            }

            // Step 3 - consolidate trees


            // Done, return minimum
            return minimumNode.Value;
        }
        
        private void Append(FibonacciHeapNode<T> heap1, FibonacciHeapNode<T> heap2)
        {
            heap1.RightSibling = heap2;
            heap2.LeftSibling = heap1;
        }
    }

    internal class FibonacciHeapNode<T> where T : IComparable
    {
        public T Value { get; }

        public int Order { get; set; }

        //public T Minimum
        //{
        //    get
        //    {
        //        return MinimumAmongSibglings.Value;
        //    }
        //}

        public FibonacciHeapNode<T> Children { get; set; }

        public FibonacciHeapNode<T> LeftSibling { get; set; }

        public FibonacciHeapNode<T> RightSibling { get; set; }

        //public FibonacciHeapNode<T> MinimumAmongSibglings { get; set; }

        public bool HasLostAChild { get; set; }

        public FibonacciHeapNode(T value)
        {
            Value = value;
            Order = 0;
            Children = null;
            LeftSibling = null;
            RightSibling = null;
        }

        //public FibonacciHeapNode<T> GetLeftmostSibling()
        //{
        //    if (LeftSibling == null)
        //    {
        //        return this;
        //    }
        //    else
        //    {
        //        return LeftSibling.GetLeftmostSibling();
        //    }
        //}

        //public FibonacciHeapNode<T> GetRightmostSibling()
        //{
        //    if (RightSibling == null)
        //    {
        //        return this;
        //    }
        //    else
        //    {
        //        return RightSibling.GetRightmostSibling();
        //    }
        //}

        public FibonacciHeapNode<T> GetMinimumNodeAmongSibglings()
        {
            FibonacciHeapNode<T> minimumNode = this;

            // Find minimum
            FibonacciHeapNode<T> leftIterator = LeftSibling;
            FibonacciHeapNode<T> rightIterator = RightSibling;

            // Look for a smaller node on the left
            while (leftIterator != null)
            {
                if (leftIterator.IsSmallerThan(minimumNode))
                {
                    minimumNode = leftIterator;
                }
                leftIterator = leftIterator.LeftSibling;
            }

            // Look for a smaller node on the right
            while (rightIterator != null)
            {
                if (rightIterator.IsSmallerThan(minimumNode))
                {
                    minimumNode = rightIterator;
                }
                rightIterator = rightIterator.RightSibling;
            }

            // Done return smallest node found
            return minimumNode;
        }

        public bool IsSmallerThan(FibonacciHeapNode<T> node)
        {
            return Value.CompareTo(node.Value) == -1;
        }
    }
}
