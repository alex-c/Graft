using System;
using System.Collections.Generic;
using Graft.Primitives;
using System.Linq;

namespace Graft
{
    public class Graph<T> : IGraph<T>
    {
        private List<IVertex<T>> Verteces { get; }

        private Dictionary<T, List<T>> Adjacency { get; }

        public Graph()
        {
            Verteces = new List<IVertex<T>>();
            Adjacency = new Dictionary<T, List<T>>();
        }

        public IVertex<T> GetFirstVertex()
        {
            return Verteces.FirstOrDefault();
        }

        public IVertex<T> GetFirstMatchingVertex(Func<IVertex<T>, bool> filter)
        {
            return Verteces.FirstOrDefault(filter);
        }

        public IEnumerable<IVertex<T>> GetVerteces()
        {
            return Verteces;
        }

        public IEnumerable<IVertex<T>> GetMatchingVerteces(Func<IVertex<T>, bool> filter)
        {
            return Verteces.Where(filter);
        }

        public IEnumerable<IVertex<T>> GetAdjacentVerteces(T value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IVertex<T>> GetAdjacentVerteces(IVertex<T> value)
        {
            throw new NotImplementedException();
        }
    }
}
