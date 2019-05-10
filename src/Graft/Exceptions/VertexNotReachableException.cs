using Graft.Primitives;
using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class VertexNotReachableException<TV> : Exception
    {
        public VertexNotReachableException() : this("The target vertex is not reachable.") { }

        public VertexNotReachableException(IVertex<TV> target) : this($"The target vertex with value {target.Value} is not reachable.") { }

        public VertexNotReachableException(string message) : base(message) { }

        public VertexNotReachableException(string message, Exception innerException) : base(message, innerException) { }

        protected VertexNotReachableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
