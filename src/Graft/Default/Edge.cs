﻿using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft.Default
{
    public class Edge<TV, TW> : IWeightedDirectedEdge<TV, TW>
    {
        private Dictionary<string, object> Attributes { get; }

        public ISet<IVertex<TV>> Verteces { get; }

        public IVertex<TV> OriginVertex { get; }

        public IVertex<TV> TargetVertex { get; }

        public TW Weight { get; }

        public Edge(Vertex<TV> originVertex, Vertex<TV> targetVertex, TW weight = default)
        {
            Attributes = new Dictionary<string, object>();
            Verteces = new HashSet<IVertex<TV>>() { originVertex, targetVertex };
            OriginVertex = originVertex;
            TargetVertex = targetVertex;
            Weight = weight;
        }

        public IVertex<TV> ConnectedVertex(IVertex<TV> vertex)
        {
            if (OriginVertex == vertex)
            {
                return TargetVertex;
            }
            else if (TargetVertex == vertex)
            {
                return OriginVertex;
            }
            else
            {
                throw new InvalidOperationException("Passed vertex is not an endpoint of this edge.");
            }
        }

        public IVertex<TV> ConnectedVertex(TV vertexValue)
        {
            if (OriginVertex.Value.Equals(vertexValue))
            {
                return TargetVertex;
            }
            else if (TargetVertex.Value.Equals(vertexValue))
            {
                return OriginVertex;
            }
            else
            {
                throw new InvalidOperationException("Passed vertex is not an endpoint of this edge.");
            }
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
                throw new KeyNotFoundException($"There is no attribute '{attribute}' set on this edge.");
            }
        }

        public TAttr GetAttribute<TAttr>(string attribute)
        {
            if (Attributes.TryGetValue(attribute, out object value))
            {
                return (TAttr)value;
            }
            else
            {
                throw new KeyNotFoundException($"There is no attribute '{attribute}' set on this edge.");
            }
        }

        public void SetAttribute(string attribute, object value)
        {
            Attributes[attribute] = value;
        }

        public bool TryGetAttribute(string attribute, out object value) => Attributes.TryGetValue(attribute, out value);

        public bool TryGetAttribute<TAttr>(string attribute, out TAttr value)
        {
            if (Attributes.TryGetValue(attribute, out object ovalue))
            {
                value = (TAttr)ovalue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        #endregion
    }
}