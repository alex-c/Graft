using System;
using System.Globalization;

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
                        throw new FormatException("Failed parsing text line, expected and integer.");
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
                        throw new FormatException("Failed parsing text line, expected two integers.");
                    }
                    break;
                case 3:
                    if (int.TryParse(components[0], out int weightedStartingVertexValue) &&
                        int.TryParse(components[1], out int weightedTargetVertexValue) &&
                        double.TryParse(components[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double weight))
                    {
                        builder.AddEdge(weightedStartingVertexValue, weightedTargetVertexValue, weight);
                    }
                    else
                    {
                        throw new FormatException("Failed parsing text line, expected two integers and a floating point value.");
                    }
                    break;
                default:
                    throw new FormatException("Input file does not match the expected format: expected 1, 2 or 3 values on this line.");
            }
        }
    }
}
