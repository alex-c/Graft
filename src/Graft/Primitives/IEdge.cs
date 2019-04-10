namespace Graft.Primitives
{
    public interface IEdge<TV, TW> : IPrimitive
    {
        IVertex<TV> TargetVertex { get; }

        TW Weight { get; }
    }
}
