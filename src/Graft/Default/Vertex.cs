using Graft.Primitives;

namespace Graft.Default
{
    public class Vertex<T> : IVertex<T>
    {
        public T Value { get; set; }

        public Vertex(T value)
        {
            Value = value;
        }
        
    }
}
