namespace Graft.Primitives
{
    public interface IVertex<T>
    {
        T Value { get; }

        bool HasTag(string key);

        void AddTag(string key, object value);
    }
}
