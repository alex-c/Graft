namespace Graft.Primitives
{
    public interface IDirectedEdge<TV> : IEdge<TV>
    {
        IVertex<TV> OriginVertex { get; }

        IVertex<TV> TargetVertex { get; }
    }
}
