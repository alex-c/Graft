namespace Graft.Default.File
{
    public class DefaultGraphTextLineParser : IGraphTextLineParser<int, double>
    {
        public void ParseLine(string line, GraphBuilder<int, double> builder)
        {
            string[] components = line.Split('\t');
            switch (components.Length)
            {
                case 1:
                    if (int.TryParse(components[0], out int numberOfVertexesToAdd))
                    {
                        builder.AddVerteces(numberOfVertexesToAdd, (n) => n);
                    }
                    else
                    {
                        // TODO
                    }
                    break;
                case 2:
                    if (int.TryParse(components[0], out int startingVertexValue) &&
                        int.TryParse(components[1], out int targetVertexValue))
                    {
                        builder.AddEdge(startingVertexValue, targetVertexValue);
                    }
                    else
                    {
                        // TODO
                    }
                    break;
                case 3:
                    if (int.TryParse(components[0], out int weightedStartingVertexValue) &&
                        int.TryParse(components[1], out int weightedTargetVertexValue) &&
                        double.TryParse(components[2], out double weight))
                    {
                        builder.AddEdge(weightedStartingVertexValue, weightedTargetVertexValue, weight);
                    }
                    else
                    {
                        // TODO
                    }
                    break;
                default:
                    // TODO
                    break;
            }
        }
    }
}
