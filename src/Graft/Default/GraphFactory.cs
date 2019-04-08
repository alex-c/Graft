using Graft.Default.File;
using System.IO;

namespace Graft.Default
{
    public class GraphFactory
    {
        Graph<TV, TW> CreateGraphFromFile<TV, TW>(string filePath, IGraphTextLineReader<TV, TW> reader, bool directed = false)
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
                        reader.ReadLine(line, builder);
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
