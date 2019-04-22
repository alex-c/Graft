using System;
using System.Collections.Generic;

namespace Graft.DataStructures
{
    public class DisjointSet<T> where T : IEquatable<T>
    {
        private Dictionary<T, DisjointSetNode<T>> Nodes { get; } 

        public DisjointSet()
        {
            Nodes = new Dictionary<T, DisjointSetNode<T>>();
        }

        public DisjointSet(IEnumerable<T> elements) : this()
        {
            foreach (T element in elements)
            {
                AddSet(element);
            }
        }

        public void AddSet(T element)
        {
            Nodes.Add(element, new DisjointSetNode<T>(element));
        }

        public T FindSet(T element)
        {
            return FindSetRootNode(element).Value;
        }

        public void Union(T element1, T element2)
        {
            DisjointSetNode<T> node1 = FindSetRootNode(element1);
            DisjointSetNode<T> node2 = FindSetRootNode(element2);

            if (!node1.Value.Equals(node2.Value))
            {
                node2.Parent = node1;
            }
        }

        private DisjointSetNode<T> FindSetRootNode(T element)
        {
            if (Nodes.TryGetValue(element, out DisjointSetNode<T> node))
            {
                while (node.HasParent())
                {
                    node = node.Parent;
                }
                return node;
            }
            else
            {
                //TODO throw exception
                throw new NotImplementedException();
            }
        }
    }

    internal class DisjointSetNode<T>
    {
        public T Value { get; }

        public DisjointSetNode<T> Parent { get; set; }

        public DisjointSetNode(T value)
        {
            Value = value;
            Parent = this;
        }

        public bool HasParent()
        {
            return Parent != this;
        }
    }
}
