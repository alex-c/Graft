namespace Graft.Primitives
{
    public interface IEdge<TV, TW>
    {
        IVertex<TV> TargetVertex { get; }

        TW Weight { get; }

        bool HasAttribute(string attribute);

        void SetAttribute(string attribute, object value);

        object GetAttribute(string attribute);

        bool TryGetAttribute(string attribute, out object value);
    }
}
