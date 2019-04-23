using Graft.Default.File;
using System;
using System.IO;

namespace Graft.Default
{
    public class GraphFactory<TV, TW> where TV : IEquatable<TV>
    {
        public Graph<TV, TW> CreateGraphFromFile(string filePath, IGraphTextLineParser<TV, TW> parser, bool directed = false)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                GraphBuilder<TV, TW> builder = new GraphBuilder<TV, TW>(directed);
                using (StreamReader streamReader = file.OpenText())
                {
                    string line = null;
                    int lineNumber = 1;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        try
                        {
                            parser.ParseLine(line, builder);
                        }
                        catch (Exception exception)
                        {
                            throw new ParserException($"Error while parsing line {lineNumber} of '{filePath}': {exception.Message}");
                        }
                        lineNumber++;
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
