using System;
using System.Collections.Generic;

namespace Graft.Default
{
    public class GraphBuilder<TV, TW> where TV : IEquatable<TV>
    {
        private bool Directed { get; }

        private Dictionary<TV, Vertex<TV>> Verteces { get; }

        private Dictionary<TV, HashSet<Edge<TV, TW>>> Edges { get; }

        public GraphBuilder(bool directed = false)
        {
            Directed = directed;
            Verteces = new Dictionary<TV, Vertex<TV>>();
            Edges = new Dictionary<TV, HashSet<Edge<TV, TW>>>();
        }

        public GraphBuilder<TV, TW> AddVertex(TV vertexValue)
        {
            Verteces[vertexValue] = new Vertex<TV>(vertexValue);
            Edges[vertexValue] = new HashSet<Edge<TV, TW>>();
            return this;
        }

        public GraphBuilder<TV, TW> AddVertex(Vertex<TV> vertex)
        {
            if (Verteces.ContainsKey(vertex.Value))
            {
                throw new InvalidOperationException($"A vertex with value {vertex.Value} has already been added to this graph.");
            }
            Verteces[vertex.Value] = vertex;
            Edges[vertex.Value] = new HashSet<Edge<TV, TW>>();
            return this;
        }

        public GraphBuilder<TV, TW> AddVerteces(IEnumerable<TV> vertexValues)
        {
            foreach (TV vertexValue in vertexValues)
            {
                AddVertex(vertexValue);
            }
            return this;
        }

        public GraphBuilder<TV, TW> AddVerteces(IEnumerable<Vertex<TV>> verteces)
        {
            foreach (Vertex<TV> vertex in verteces)
            {
                AddVertex(vertex);
            }
            return this;
        }

        public GraphBuilder<TV, TW> AddVerteces(int numberOfVertecesToAdd, Func<int, TV> vertexValueProvider)
        {
            for (int i = 0; i < numberOfVertecesToAdd; i++)
            {
                AddVertex(vertexValueProvider(i));
            }
            return this;
        }

        public GraphBuilder<TV, TW> AddEdge(TV startingVertexValue, TV targetVertexValue, TW weight = default)
        {
            if (Verteces.ContainsKey(startingVertexValue) && Verteces.ContainsKey(targetVertexValue))
            {
                Edge<TV, TW> newEdge = new Edge<TV, TW>(Verteces[startingVertexValue], Verteces[targetVertexValue], weight);
                Edges[startingVertexValue].Add(newEdge);
                if (!Directed)
                {
                    Edges[targetVertexValue].Add(newEdge);
                }
            }
            else
            {
                throw new InvalidOperationException($"One or more of the passed verteces are not verteces of this graph.");
            }
            return this;
        }

        public GraphBuilder<TV, TW> AddEdge(Edge<TV, TW> edge)
        {
            if (Verteces.ContainsKey(edge.OriginVertex.Value) && Verteces.ContainsKey(edge.TargetVertex.Value))
            {
                Edges[edge.OriginVertex.Value].Add(edge);
                if (!Directed)
                {
                    Edges[edge.TargetVertex.Value].Add(edge);
                }
            }
            else
            {
                throw new InvalidOperationException($"One or more of the passed verteces are not verteces of this graph.");
            }
            return this;
        }

        public Graph<TV, TW> Build()
        {
            return new Graph<TV, TW>(new HashSet<Vertex<TV>>(Verteces.Values), Edges, Directed);
        }
    }
}
