namespace Graft.Primitives
{
    public interface IEdge<T>
    {
        T GetWeight();

        void HasAttribute(string attribute);

        void SetAttribute(string attribute, object value);

        object GetAttribute(string attribute);
    }
}
