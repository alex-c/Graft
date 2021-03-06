﻿using System;
using System.Collections.Generic;
using Graft.Primitives;
using System.Linq;
using Graft.Exceptions;

namespace Graft.Default
{
    public class Graph<TV, TW> : IWeightedGraph<TV, TW> where TV: IEquatable<TV>
    {
        private HashSet<Vertex<TV>> Verteces { get; }

        private Dictionary<TV, HashSet<Edge<TV, TW>>> Adjacency { get; }

        public bool IsDirected { get; }

        public int VertexCount { get; }

        public Graph(bool isDirected = false) : this(new HashSet<Vertex<TV>>(), new Dictionary<TV, HashSet<Edge<TV, TW>>>(), isDirected) { }

        public Graph(HashSet<Vertex<TV>> verteces, Dictionary<TV, HashSet<Edge<TV, TW>>> adjacency, bool isDirected = false)
        {
            Verteces = verteces;
            Adjacency = adjacency;
            VertexCount = verteces.Count;
            IsDirected = isDirected;
        }

        #region Vertex access

        public IVertex<TV> GetFirstVertex()
        {
            return Verteces.FirstOrDefault();
        }

        public IVertex<TV> GetFirstMatchingVertex(Func<IVertex<TV>, bool> filter)
        {
            return Verteces.FirstOrDefault(filter);
        }

        public IEnumerable<IVertex<TV>> GetAllVerteces()
        {
            return Verteces;
        }

        public IEnumerable<IVertex<TV>> GetAllMatchingVerteces(Func<IVertex<TV>, bool> filter)
        {
            return Verteces.Where(filter);
        }

        public IEnumerable<IVertex<TV>> GetAdjacentVerteces(IVertex<TV> vertex)
        {
            return GetAdjacentVerteces(vertex.Value);
        }

        public IEnumerable<IVertex<TV>> GetAdjacentVerteces(TV vertexValue)
        {
            if (ContainsVertex(vertexValue))
            {
                Adjacency.TryGetValue(vertexValue, out HashSet<Edge<TV, TW>> edges);
                IEnumerable<IVertex<TV>> targetVerteces = null;
                if (IsDirected)
                {
                    targetVerteces = edges.Select(e => e.TargetVertex);
                }
                else
                {
                    targetVerteces = edges.Select(e => e.ConnectedVertex(vertexValue));
                }
                return targetVerteces;
            }
            else
            {
                throw new InvalidOperationException("The passed vertex is not a vertex of this graph.");
            }
        }

        public bool ContainsVertex(IVertex<TV> vertex)
        {
            return Verteces.Contains(vertex);
        }

        public bool ContainsVertex(TV vertexValue)
        {
            return Adjacency.Keys.Contains(vertexValue);
        }

        #endregion

        #region Edge access

        public IEnumerable<IWeightedEdge<TV, TW>> GetAllEdges()
        {
            HashSet<IWeightedDirectedEdge<TV, TW>> allEdges = new HashSet<IWeightedDirectedEdge<TV, TW>>();
            foreach (HashSet<Edge<TV, TW>> edgeList in Adjacency.Values)
            {
                allEdges.UnionWith(edgeList);
            }
            return allEdges;
        }

        IEnumerable<IEdge<TV>> IGraph<TV>.GetAllEdges()
        {
            return GetAllEdges();
        }

        public IEnumerable<IWeightedEdge<TV, TW>> GetEdgesOfVertex(IVertex<TV> vertex)
        {
            if (Adjacency.TryGetValue(vertex.Value, out HashSet<Edge<TV, TW>> edges))
            {
                return edges;
            }
            else
            {
                throw new InvalidOperationException("The passed vertex is not a vertex of this graph.");
            }
        }

        IEnumerable<IEdge<TV>> IGraph<TV>.GetEdgesOfVertex(IVertex<TV> vertex)
        {
            return GetEdgesOfVertex(vertex);
        }

        public IWeightedEdge<TV, TW> GetEdgeBetweenVerteces(IVertex<TV> source, IVertex<TV> target)
        {
            if (ContainsVertex(source) && ContainsVertex(target))
            {
                Edge<TV, TW> edge = Adjacency[source.Value].FirstOrDefault(t => t.ConnectedVertex(source) == target);
                return edge ?? throw new VertecesNotConnectedException<TV>(source, target);
            }
            else
            {
                throw new InvalidOperationException("At least one of the passed verteces is not a vertex of this graph.");
            }
        }

        IEdge<TV> IGraph<TV>.GetEdgeBetweenVerteces(IVertex<TV> source, IVertex<TV> target)
        {
            return GetEdgeBetweenVerteces(source, target);
        }

        public IWeightedEdge<TV, TW> GetEdgeBetweenVerteces(TV sourceVertexValue, TV targetVertexValue)
        {
            if (ContainsVertex(sourceVertexValue) && ContainsVertex(targetVertexValue))
            {
                Edge<TV, TW> edge = Adjacency[sourceVertexValue].FirstOrDefault(t => t.ConnectedVertex(sourceVertexValue).Value.Equals(targetVertexValue));
                return edge ?? throw new VertecesNotConnectedException<TV>(sourceVertexValue, targetVertexValue);
            }
            else
            {
                throw new InvalidOperationException("At least one of the passed verteces is not a vertex of this graph.");
            }
        }

        IEdge<TV> IGraph<TV>.GetEdgeBetweenVerteces(TV sourceVertexValue, TV targetVertexValue)
        {
            return GetEdgeBetweenVerteces(sourceVertexValue, targetVertexValue);
        }

        public bool AreVertecesConnected(TV sourceVertexValue, TV targetVertexValue)
        {
            return Adjacency[sourceVertexValue].FirstOrDefault(t => t.ConnectedVertex(sourceVertexValue).Value.Equals(targetVertexValue)) != null;
        }

        #endregion
    }
}
