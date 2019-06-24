using System;
using System.Runtime.Serialization;

namespace Graft.Exceptions
{
    public class GraphNotDirectedException : Exception
    {
        public GraphNotDirectedException() : this("This graph is not directed.") { }

        public GraphNotDirectedException(string message) : base(message) { }

        public GraphNotDirectedException(string message, Exception innerException) : base(message, innerException) { }

        protected GraphNotDirectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
