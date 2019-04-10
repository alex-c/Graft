using System;
using System.Collections.Generic;
using Graft.Primitives;
using System.Linq;

namespace Graft.Default
{
    public class Graph<TV, TW> : IGraph<TV>
    {
        private List<Vertex<TV>> Verteces { get; }

        private Dictionary<TV, List<Edge<TV, TW>>> Adjacency { get; }

        private Dictionary<string, object> Attributes { get; }

        public bool IsDirected { get; }

        public Graph(bool isDirected = false) : this(new List<Vertex<TV>>(), new Dictionary<TV, List<Edge<TV, TW>>>(), isDirected) { }

        public Graph(List <Vertex<TV>> verteces, Dictionary<TV, List<Edge<TV, TW>>> adjacency, bool isDirected = false)
        {
            Verteces = verteces;
            Adjacency = adjacency;
            Attributes = new Dictionary<string, object>();
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

        public IEnumerable<IVertex<TV>> GetVerteces()
        {
            return Verteces;
        }

        public IEnumerable<IVertex<TV>> GetMatchingVerteces(Func<IVertex<TV>, bool> filter)
        {
            return Verteces.Where(filter);
        }

        public IEnumerable<IVertex<TV>> GetAdjacentVerteces(TV value)
        {
            if (Contains(value))
            {
                Adjacency.TryGetValue(value, out List<Edge<TV, TW>> edges);
                return edges.Select(e => e.TargetVertex);
            }
            else
            {
                throw new NotImplementedException(); // TODO
            }
        }

        public IEnumerable<IVertex<TV>> GetAdjacentVerteces(IVertex<TV> vertex)
        {
            return GetAdjacentVerteces(vertex.Value);
        }

        private bool Contains(TV value)
        {
            return Adjacency.Keys.Contains(value);
        }

        #endregion

        #region Attributes

        public bool HasAttribute(string attribute) => Attributes.ContainsKey(attribute);

        public object GetAttribute(string attribute)
        {
            if (Attributes.TryGetValue(attribute, out object value))
            {
                return value;
            }
            else
            {
                throw new NotImplementedException(); // TODO
            }
        }

        public void SetAttribute(string attribute, object value)
        {
            Attributes[attribute] = value;
        }

        public bool TryGetAttribute(string attribute, out object value) => Attributes.TryGetValue(attribute, out value);

        #endregion

    }
}
