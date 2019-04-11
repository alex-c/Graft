namespace Graft.Primitives
{
    public interface IWeightedEdge<TV, TW> : IEdge<TV>
    {
        TW Weight { get; }
    }
}
