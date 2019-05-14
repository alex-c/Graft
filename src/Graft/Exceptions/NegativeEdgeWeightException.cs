using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class NegativeEdgeWeightException : Exception
    {
        public NegativeEdgeWeightException() : this("An edge has a negative weight.") { }

        public NegativeEdgeWeightException(string message) : base(message) { }

        public NegativeEdgeWeightException(string message, Exception innerException) : base(message, innerException) { }

        protected NegativeEdgeWeightException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
