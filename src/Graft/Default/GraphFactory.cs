using Graft.Default.File;
using System.IO;

namespace Graft.Default
{
    public class GraphFactory
    {
        public Graph<TV, TW> CreateGraphFromFile<TV, TW>(string filePath, IGraphTextLineParser<TV, TW> parser, bool directed = false)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(directed);
                using (StreamReader streamReader = file.OpenText())
                {
                    string line = null;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        parser.ParseLine(line, builder);
                    }
                }
                return builder.Build();
            }
            else
            {
                throw new FileNotFoundException("Graph input file not found.", filePath);
            }
        }
    }
}
