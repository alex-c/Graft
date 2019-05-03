using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class GraphNotCompleteException : Exception
    {
        public GraphNotCompleteException() : this("The graph is not complete.") { }

        public GraphNotCompleteException(string message) : base(message) { }

        public GraphNotCompleteException(string message, Exception innerException) : base(message, innerException) { }

        protected GraphNotCompleteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
