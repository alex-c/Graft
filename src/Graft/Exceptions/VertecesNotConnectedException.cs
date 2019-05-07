using Graft.Primitives;
using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class VertecesNotConnectedException<TV> : Exception
    {
        public VertecesNotConnectedException() : this("The verteces are not connected in this graph.") { }

        public VertecesNotConnectedException(IVertex<TV> source, IVertex<TV> target) : this(source.Value, target.Value) { }

        public VertecesNotConnectedException(TV sourceVertexValue, TV targetVertexValue)
            : this($"The verteces with values {sourceVertexValue} and {targetVertexValue} are not connected in this graph.") { }

        public VertecesNotConnectedException(string message) : base(message) { }

        public VertecesNotConnectedException(string message, Exception innerException) : base(message, innerException) { }

        protected VertecesNotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
