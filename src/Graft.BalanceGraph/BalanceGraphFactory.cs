using Graft.Default;
using Graft.Default.File;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Graft.BalanceGraph
{
    public class BalanceGraphFactory
    {
        public Graph<int, double> CreateGraphFromFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                List<Vertex<int>> verteces = new List<Vertex<int>>();
                List<Edge<int, double>> edges = new List<Edge<int, double>>();

                using (StreamReader streamReader = file.OpenText())
                {
                    int lineNumber = 1;
                    int numberOfVerteces = 0;

                    // Get number of verteces from first line
                    string line = streamReader.ReadLine();
                    if (line != null)
                    {
                        if (int.TryParse(line, out numberOfVerteces))
                        {
                            for (int i = 0; i < numberOfVerteces; i++)
                            {
                                verteces.Add(new Vertex<int>(i));
                            }
                        }
                        else
                        {
                            throw new ParserException($"Error while parsing file '{filePath}': expected vertex count on line 1.");
                        }
                    }
                    else
                    {
                        throw new ParserException($"Error while parsing file '{filePath}': file empty.");
                    }
                    lineNumber++;

                    // Read and set balances
                    for (int i = 0; i < numberOfVerteces; i++)
                    {
                        if ((line = streamReader.ReadLine()) != null)
                        {
                            if (double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out double balance))
                            {
                                verteces[i].SetAttribute(Constants.BALANCE, balance);
                            }
                            else
                            {
                                throw new ParserException($"Error while parsing file '{filePath}': expected a balance value at line {lineNumber}.");
                            }
                        }
                        else
                        {
                            throw new ParserException($"Error while parsing file '{filePath}': file ended unexpectedly at line {lineNumber}.");
                        }
                        lineNumber++;
                    }

                    // Read edges with and capacity as weight and set costs
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string[] components = line.Split('\t');
                        if (components.Length != 4)
                        {
                            throw new ParserException($"Error while parsing file '{filePath}': expected 4 elements at line {lineNumber}.");
                        }
                        if (int.TryParse(components[0], out int weightedStartingVertexValue) &&
                            int.TryParse(components[1], out int weightedTargetVertexValue) &&
                            double.TryParse(components[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double costs) &&
                            double.TryParse(components[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double weight))
                        {
                            Edge<int, double> edge = new Edge<int, double>(verteces[weightedStartingVertexValue], verteces[weightedTargetVertexValue], weight);
                            edge.SetAttribute(Constants.COSTS, costs);
                            edges.Add(edge);
                        }
                        else
                        {
                            throw new FormatException("Failed parsing text line, expected two integers and a floating point value.");
                        }
                    }
                }

                // Build and return graph
                GraphBuilder<int, double> builder = new GraphBuilder<int, double>(true);
                builder.AddVerteces(verteces);
                builder.AddEdges(edges);
                return builder.Build();
            }
            else
            {
                throw new FileNotFoundException("Graph input file not found.", filePath);
            }
        }
    }
}
