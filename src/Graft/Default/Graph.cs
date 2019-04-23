using System;
using System.Collections.Generic;
using Graft.Primitives;
using System.Linq;

namespace Graft.Default
{
    public class Graph<TV, TW> : IWeightedGraph<TV, TW> where TV: IEquatable<TV>
    {
        private HashSet<Vertex<TV>> Verteces { get; }

        private Dictionary<TV, HashSet<Edge<TV, TW>>> Adjacency { get; }

        public bool IsDirected { get; }

        public Graph(bool isDirected = false) : this(new HashSet<Vertex<TV>>(), new Dictionary<TV, HashSet<Edge<TV, TW>>>(), isDirected) { }

        public Graph(HashSet<Vertex<TV>> verteces, Dictionary<TV, HashSet<Edge<TV, TW>>> adjacency, bool isDirected = false)
        {
            Verteces = verteces;
            Adjacency = adjacency;
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
                throw new NotImplementedException(); // TODO
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
            throw new NotImplementedException();
        }

        IEnumerable<IEdge<TV>> IGraph<TV>.GetEdgesOfVertex(IVertex<TV> vertex)
        {
            return GetEdgesOfVertex(vertex);
        }

        #endregion
    }
}
