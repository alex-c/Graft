using Graft.Primitives;
using System;
using System.Collections.Generic;

namespace Graft
{
    public interface IGraph<T>
    {
        IVertex<T> GetFirstVertex();

        IVertex<T> GetFirstMatchingVertex(Func<IVertex<T>, bool> filter);

        IEnumerable<IVertex<T>> GetVerteces();

        IEnumerable<IVertex<T>> GetMatchingVerteces(Func<IVertex<T>, bool> filter);

        IEnumerable<IVertex<T>> GetAdjacentVerteces(T value);

        IEnumerable<IVertex<T>> GetAdjacentVerteces(IVertex<T> value);
    }
}
