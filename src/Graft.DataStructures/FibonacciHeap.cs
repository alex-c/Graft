using System;
using System.Collections.Generic;
using System.Linq;

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

        public (TE, TP) ExtractMinimum()
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
                minimumNode.Children.Parent = null;

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
            ConsolidateTrees();

            // Update count
            Count--;

            // Done, return minimum
            return (minimumNode.Element, minimumNode.Priority);
        }

        public void DecreaseKey(TE element, TP priority)
        {
            FibonacciHeapNode<TE, TP> node = GetNode(element);
            node.Priority = priority;

            // If the heap order is violated, extract the subtree and move it to the main tree list
            if (node.Parent != null && node.IsSmallerThan(node.Parent))
            {
                ExtractTree(node);
            }
            Root = Root.GetMinimumNodeAmongSibglings();
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

        private void ConsolidateTrees()
        {
            if (Root != null)
            {
                // Keep track of which trees have which rank
                Dictionary<int, FibonacciHeapNode<TE, TP>> RankRoots = new Dictionary<int, FibonacciHeapNode<TE, TP>>();

                // Check each tree
                foreach (FibonacciHeapNode<TE, TP> node in Root.GetAllSiblings())
                {
                    FibonacciHeapNode<TE, TP> newTreeRoot = node;
                    int rank = newTreeRoot.GetRank();

                    // If there is already a tree with the same rank, merge them
                    while (RankRoots.ContainsKey(rank))
                    {
                        FibonacciHeapNode<TE, TP> nodeToLink = RankRoots[rank];
                        RankRoots.Remove(rank);
                        newTreeRoot = MergeHeaps(newTreeRoot, nodeToLink);
                        rank = newTreeRoot.GetRank();
                    }

                    // The tree - newly merged or not - is currently the only known tree with that rank
                    RankRoots.Add(rank, newTreeRoot);
                }
            }
        }

        private void Append(FibonacciHeapNode<TE, TP> heap1, FibonacciHeapNode<TE, TP> heap2)
        {
            var rightEdge = heap1.GetRightmostSibling();
            var leftEdge = heap2.GetLeftmostSibling();
            rightEdge.RightSibling = leftEdge;
            leftEdge.LeftSibling = rightEdge;
        }

        private FibonacciHeapNode<TE, TP> MergeHeaps(FibonacciHeapNode<TE, TP> heap1, FibonacciHeapNode<TE, TP> heap2)
        {
            if (heap1.IsSmallerThan(heap2))
            {
                return MergeInto(heap1, heap2);
            }
            else
            {
                return MergeInto(heap2, heap1);
            }
        }

        private FibonacciHeapNode<TE, TP> MergeInto(FibonacciHeapNode<TE, TP> root, FibonacciHeapNode<TE, TP> tree)
        {
            // Extract tree to merge from its siblings
            if (tree.LeftSibling != null)
            {
                tree.LeftSibling.RightSibling = tree.RightSibling;
            }
            if (tree.RightSibling != null)
            {
                tree.RightSibling.LeftSibling = tree.LeftSibling;
            }
            tree.LeftSibling = null;
            tree.RightSibling = null;

            // Merge tree into children of root
            if (root.Children == null)
            {
                root.Children = tree;
            }
            else
            {
                Append(root.Children, tree);
            }
            tree.Parent = root;

            // Return the resulting heap
            return root;
        }

        private void ExtractTree(FibonacciHeapNode<TE, TP> node)
        {
            FibonacciHeapNode<TE, TP> parent = node.Parent;

            // Extract
            node.Parent = null;
            parent.Children = node.LeftSibling ?? node.RightSibling;
            if (node.LeftSibling != null)
            {
                node.LeftSibling.RightSibling = node.RightSibling;
            }
            if (node.RightSibling != null)
            {
                node.RightSibling.LeftSibling = node.LeftSibling;
            }
            node.LeftSibling = null;
            node.RightSibling = null;

            // Move to root tree list
            Append(Root, node);

            // If the parent had already lost a child extract it too
            if (parent != null)
            {
                if (parent.HasLostAChild)
                {
                    parent.HasLostAChild = false;
                    ExtractTree(parent);
                }
                else
                {
                    parent.HasLostAChild = true;
                }
            }
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

        public FibonacciHeapNode<TE, TP> Parent { get; set; }

        public FibonacciHeapNode<TE, TP> Children { get; set; }

        public FibonacciHeapNode<TE, TP> LeftSibling { get; set; }

        public FibonacciHeapNode<TE, TP> RightSibling { get; set; }

        public bool HasLostAChild { get; set; }

        public FibonacciHeapNode(TE element, TP priority)
        {
            Element = element;
            Priority = priority;
            Parent = null;
            Children = null;
            LeftSibling = null;
            RightSibling = null;
            HasLostAChild = false;
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
            HashSet<FibonacciHeapNode<TE, TP>> siblings = new HashSet<FibonacciHeapNode<TE, TP>>
            {
                this
            };

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

        public FibonacciHeapNode<TE, TP> GetLeftmostSibling()
        {
            FibonacciHeapNode<TE, TP> iterator = this;
            
            while (iterator.LeftSibling != null)
            {
                iterator = iterator.LeftSibling;
            }

            return iterator;
        }

        public FibonacciHeapNode<TE, TP> GetRightmostSibling()
        {
            FibonacciHeapNode<TE, TP> iterator = this;

            while (iterator.RightSibling != null)
            {
                iterator = iterator.RightSibling;
            }

            return iterator;
        }

        public int GetRank()
        {
            if (Children == null)
            {
                return 0;
            }
            else
            {
                HashSet<FibonacciHeapNode<TE, TP>> siblings = Children.GetAllSiblings();
                return 1 + siblings.Max(n => n.GetRank());
            }
        }

        public bool IsSmallerThan(FibonacciHeapNode<TE, TP> node)
        {
            return Priority.CompareTo(node.Priority) == -1;
        }
    }
}
