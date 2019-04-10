namespace Graft.Primitives
{
    public interface IVertex<T> : IPrimitive
    {
        T Value { get; }
    }
}
