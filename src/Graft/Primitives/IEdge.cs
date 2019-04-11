namespace Graft.Primitives
{
    public interface IEdge<TV> : IPrimitive
    {
        bool IsDirected { get; }

        IVertex<TV> OriginVertex { get; }

        IVertex<TV> TargetVertex { get; }
    }
}
