namespace Graft.Default.File
{
    public interface IGraphTextLineReader<TV, TW>
    {
        void ReadLine(string line, GraphBuilder<TV, TW> builder);
    }
}
