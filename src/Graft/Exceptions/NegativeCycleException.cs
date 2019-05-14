using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class NegativeCycleException : Exception
    {
        public NegativeCycleException() : this("The graph contains a negative cycle.") { }

        public NegativeCycleException(string message) : base(message) { }

        public NegativeCycleException(string message, Exception innerException) : base(message, innerException) { }

        protected NegativeCycleException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
