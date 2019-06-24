using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class NoBFlowException : Exception
    {
        public NoBFlowException() : this("This graph does not contain any valid b-flow.") { }

        public NoBFlowException(string message) : base(message) { }

        public NoBFlowException(string message, Exception innerException) : base(message, innerException) { }

        protected NoBFlowException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
