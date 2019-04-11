using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Default
{
    public class Edge<TV, TW> : IWeightedEdge<TV, TW>
    {
        private Dictionary<string, object> Attributes { get; }

        public bool IsDirected { get; }

        public IVertex<TV> OriginVertex { get; }

        public IVertex<TV> TargetVertex { get; }

        public TW Weight { get; }

        public Edge(Vertex<TV> originVertex, Vertex<TV> targetVertex, bool directed = false, TW weight = default(TW))
        {
            Attributes = new Dictionary<string, object>();
            IsDirected = directed;
            OriginVertex = originVertex;
            TargetVertex = targetVertex;
            Weight = weight;
        }

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