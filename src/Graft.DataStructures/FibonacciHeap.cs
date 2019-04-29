using System;
using System.Collections.Generic;
using System.Text;

namespace Graft.DataStructures
{
    public class FibonacciHeap<TE, TP> where TP : IComparable
    {
        private FibonacciHeapNode<TE, TP> Root { get; set; }

        public bool Empty => Root == null;

        public int Count { get; private set; }
        
        public TE Minimum
        {
            get
            {
                if (Empty)
                {
                    throw new InvalidOperationException("Heap is empty.");
                }
                return Root.Element;
            }
        }

        public FibonacciHeap()
        {
            Count = 0;
        }

        public void Insert(TE element, TP priority)
        {
            var newNode = new FibonacciHeapNode<TE, TP>(element, priority);
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
            Count++;
        }

        public TE ExtractMinimum()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            // Step 1 - remove minimum element
            // Get minimum node and set root pointer to one of it's siblings (could be null if there are none)
            FibonacciHeapNode<TE, TP> minimumNode = Root;
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
            // TODO: implement tree consolidation

            // Update count
            Count--;

            // Done, return minimum
            return minimumNode.Element;
        }

        public void DecreaseKey(TE element, TP priority)
        {
            // TODO: implement key decreasing
            throw new NotImplementedException();
        }

        public bool Contains(TE element)
        {
            return GetNode(element) != null;
        }

        public TP GetPriorityOf(TE element)
        {
            var node = GetNode(element);
            if (node != null)
            {
                return node.Priority;
            }
            else
            {
                throw new InvalidOperationException("Can't get priority of an element that is not in the heap.");
            }
        }
        
        private void Append(FibonacciHeapNode<TE, TP> heap1, FibonacciHeapNode<TE, TP> heap2)
        {
            heap1.RightSibling = heap2;
            heap2.LeftSibling = heap1;
        }

        private FibonacciHeapNode<TE, TP> GetNode(TE element)
        {
            if (Empty)
            {
                return null;
            }
            else
            {
                // Check nodes in breadth first search order: siblings first, then children
                Queue<FibonacciHeapNode<TE, TP>> nodesToCheck = new Queue<FibonacciHeapNode<TE, TP>>(Root.GetAllSiblings());
                while (nodesToCheck.Count > 0)
                {
                    var node = nodesToCheck.Dequeue();
                    if (node.Element.Equals(element))
                    {
                        // Element is contained!
                        return node;
                    }
                    else if (node.Children != null)
                    {
                        // Enqueue child nodes
                        foreach (FibonacciHeapNode<TE, TP> childNode in node.Children.GetAllSiblings())
                        {
                            nodesToCheck.Enqueue(childNode);
                        }
                    }
                }
                // If we got here, the element is not in the heap!
                return null;
            }
        }
    }

    internal class FibonacciHeapNode<TE, TP> where TP : IComparable
    {
        public TE Element { get; }

        public TP Priority { get; set; }

        public int Order { get; set; }

        public FibonacciHeapNode<TE, TP> Children { get; set; }

        public FibonacciHeapNode<TE, TP> LeftSibling { get; set; }

        public FibonacciHeapNode<TE, TP> RightSibling { get; set; }

        public bool HasLostAChild { get; set; }

        public FibonacciHeapNode(TE element, TP priority)
        {
            Element = element;
            Priority = priority;
            Order = 0;
            Children = null;
            LeftSibling = null;
            RightSibling = null;
        }

        public FibonacciHeapNode<TE, TP> GetMinimumNodeAmongSibglings()
        {
            FibonacciHeapNode<TE, TP> minimumNode = this;

            // Find minimum
            FibonacciHeapNode<TE, TP> leftIterator = LeftSibling;
            FibonacciHeapNode<TE, TP> rightIterator = RightSibling;

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

        public HashSet<FibonacciHeapNode<TE, TP>> GetAllSiblings()
        {
            HashSet<FibonacciHeapNode<TE, TP>> siblings = new HashSet<FibonacciHeapNode<TE, TP>>();
            siblings.Add(this);

            // Find siblings
            FibonacciHeapNode<TE, TP> leftIterator = LeftSibling;
            FibonacciHeapNode<TE, TP> rightIterator = RightSibling;
            
            // Enumerate siblings on the left
            while (leftIterator != null)
            {
                siblings.Add(leftIterator);
                leftIterator = leftIterator.LeftSibling;
            }

            // Enumerate siblings on the right
            while (rightIterator != null)
            {
                siblings.Add(rightIterator);
                rightIterator = rightIterator.RightSibling;
            }

            return siblings;
        }

        public bool IsSmallerThan(FibonacciHeapNode<TE, TP> node)
        {
            return Priority.CompareTo(node.Priority) == -1;
        }
    }
}
