using System;
using System.Collections.Generic;

namespace Graft.DataStructures
{
    /// <summary>
    /// A disjoint set data structure for tracking a set of elements partitioned into a number of disjoint subsets.
    /// </summary>
    /// <typeparam name="T">Type of values in the sets.</typeparam>
    public class DisjointSet<T> where T : IEquatable<T>
    {
        private Dictionary<T, DisjointSetNode<T>> Nodes { get; } 

        /// <summary>
        /// Sets up a disjoint set data structure without any elements.
        /// </summary>
        public DisjointSet()
        {
            Nodes = new Dictionary<T, DisjointSetNode<T>>();
        }

        /// <summary>
        /// Sets up a disjoint set data structure with n subsets of 1 element each.
        /// </summary>
        /// <param name="elements">Elements to create subsets for.</param>
        public DisjointSet(IEnumerable<T> elements) : this()
        {
            foreach (T element in elements)
            {
                AddSet(element);
            }
        }

        /// <summary>
        /// Adds a new subset with a new element.
        /// </summary>
        /// <param name="element">Element to create a new subset for.</param>
        public void AddSet(T element)
        {
            Nodes.Add(element, new DisjointSetNode<T>(element));
        }

        /// <summary>
        /// Finds the representative element for the subset in which the passed element is contained.
        /// </summary>
        /// <param name="element">Element for which to find the representative subset element.</param>
        /// <returns>Returns the representative subset element.</returns>
        public T FindSet(T element)
        {
            return FindSetRootNode(element).Value;
        }

        /// <summary>
        /// Unites the two subsets containing the passed elements. Does nothing if the passed elements
        /// are already contained in one and the same subset.
        /// </summary>
        /// <param name="element1">First element to unite the subset of.</param>
        /// <param name="element2">Second element to unite the subset of.</param>
        public void Union(T element1, T element2)
        {
            DisjointSetNode<T> node1 = FindSetRootNode(element1);
            DisjointSetNode<T> node2 = FindSetRootNode(element2);

            if (!node1.Value.Equals(node2.Value))
            {
                node2.Parent = node1;
            }
        }
        
        /// <summary>
        /// Finds the root node of the subset in which the passed element is contained. That root node
        /// represents that subset.
        /// </summary>
        /// <param name="element">The elemenet for which to find the subset root node.</param>
        /// <returns></returns>
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
                throw new InvalidOperationException("Can't find the subset root node of an element which is not contained in the disjoint set.");
            }
        }
    }

    /// <summary>
    /// A node of a subset in the disjoint set data structure. Exposes the node value and parent,
    /// and allows changing the parent.
    /// </summary>
    /// <typeparam name="T">Type of the value associated with the set node.</typeparam>
    internal class DisjointSetNode<T>
    {
        /// <summary>
        /// Value of the set node.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Parent of the set node. Can be itself, in which case it is considered to not have a parent.
        /// </summary>
        public DisjointSetNode<T> Parent { get; set; }

        /// <summary>
        /// Creates a new and parentless set node.
        /// </summary>
        /// <param name="value">Value of the node.</param>
        public DisjointSetNode(T value)
        {
            Value = value;
            Parent = this;
        }

        /// <summary>
        /// Checks whether the set node has a parent.
        /// </summary>
        /// <returns>Returns whether the set node has a parent.</returns>
        public bool HasParent()
        {
            return Parent != this;
        }
    }
}
