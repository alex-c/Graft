using System;

namespace Graft.Default.File
{
    public interface IGraphTextLineParser<TV, TW> where TV : IEquatable<TV>
    {
        void ParseLine(string line, GraphBuilder<TV, TW> builder);
    }
}
