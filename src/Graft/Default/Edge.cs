using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Default
{
    public class Edge<TV, TW> : IEdge<TV, TW>
    {
        public IVertex<TV> TargetVertex { get; }

        public TW Weight { get; }

        private Dictionary<string, object> Attributes { get; }

        public Edge(Vertex<TV> vertex, TW weight = default(TW))
        {
            TargetVertex = vertex;
            Weight = weight;
            Attributes = new Dictionary<string, object>();
        }

        public bool HasAttribute(string attribute) => Attributes.ContainsKey(attribute);

        public void SetAttribute(string attribute, object value)
        {
            Attributes[attribute] = value;
        }

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

        public bool TryGetAttribute(string attribute, out object value) => Attributes.TryGetValue(attribute, out value);
    }
}