namespace Graft.Primitives
{
    public interface IPrimitive
    {
        bool HasAttribute(string attribute);

        object GetAttribute(string attribute);

        T GetAttribute<T>(string attribute);

        void SetAttribute(string attribute, object value);

        bool TryGetAttribute(string attribute, out object value);

        bool TryGetAttribute<T>(string attribute, out T value);
    }
}
