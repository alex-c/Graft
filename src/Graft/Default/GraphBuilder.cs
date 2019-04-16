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

        public GraphBuilder<TV, TW> AddVerteces(IEnumerable<TV> vertexValues)
        {
            foreach (TV vertexValue in vertexValues)
            {
                AddVertex(vertexValue);
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

        public GraphBuilder<TV, TW> AddEdge(TV startingVertexValue, TV targetVertexValue, TW weight = default(TW))
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
                throw new NotImplementedException(); //TODO
            }
            return this;
        }

        public Graph<TV, TW> Build()
        {
            return new Graph<TV, TW>(new HashSet<Vertex<TV>>(Verteces.Values), Edges);
        }
    }
}
