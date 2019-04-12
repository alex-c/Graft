using System.Collections.Generic;

namespace Graft.Primitives
{
    public interface IEdge<TV> : IPrimitive
    {
        ISet<IVertex<TV>> Verteces { get; }

        IVertex<TV> ConnectedVertex(IVertex<TV> vertex);

        IVertex<TV> ConnectedVertex(TV vertexValue);
    }
}
