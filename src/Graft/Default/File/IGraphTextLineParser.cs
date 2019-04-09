namespace Graft.Default.File
{
    public interface IGraphTextLineParser<TV, TW>
    {
        void ParseLine(string line, GraphBuilder<TV, TW> builder);
    }
}
