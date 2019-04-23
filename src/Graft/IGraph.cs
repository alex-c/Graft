using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft
{
    public interface IGraph<T> where T : IEquatable<T>
    {
        bool IsDirected { get; }

        int VertexCount { get; }

        #region Vertex access

        IVertex<T> GetFirstVertex();

        IVertex<T> GetFirstMatchingVertex(Func<IVertex<T>, bool> filter);

        IEnumerable<IVertex<T>> GetAllVerteces();

        IEnumerable<IVertex<T>> GetAllMatchingVerteces(Func<IVertex<T>, bool> filter);

        IEnumerable<IVertex<T>> GetAdjacentVerteces(IVertex<T> vertex);

        IEnumerable<IVertex<T>> GetAdjacentVerteces(T vertexValue);

        bool ContainsVertex(IVertex<T> vertex);

        bool ContainsVertex(T vertexValue);

        #endregion

        #region Edge access

        IEnumerable<IEdge<T>> GetAllEdges();

        IEnumerable<IEdge<T>> GetEdgesOfVertex(IVertex<T> vertex);

        #endregion
    }
}