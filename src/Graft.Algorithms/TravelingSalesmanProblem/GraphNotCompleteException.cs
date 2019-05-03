using System;
using System.Runtime.Serialization;

namespace Graft.Algorithms.TravelingSalesmanProblem
{
    public class GraphNotCompleteException : Exception
    {
        public GraphNotCompleteException() { }

        public GraphNotCompleteException(string message) : base(message) { }

        public GraphNotCompleteException(string message, Exception innerException) : base(message, innerException) { }

        protected GraphNotCompleteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
