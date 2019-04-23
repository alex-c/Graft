using Graft.Default;
using Graft.Default.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Graft.Labs.MinimumSpanningTree
{
    public class TestRunner
    {
        private const string PATH = "./graphs/";

        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        private HashSet<string> Files { get; }

        private Dictionary<string, IWeightedGraph<int, double>> Graphs { get; }

        public TestRunner()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
            Files = new HashSet<string>();
            Graphs = new Dictionary<string, IWeightedGraph<int, double>>();
        }

        public void AddFile(string file)
        {
            Files.Add(file);
        }

        public void RunTests()
        {
            ReadGraphsFromFile();
            Console.WriteLine("Run minimum spanning tree tests...");
            foreach (KeyValuePair<string, IWeightedGraph<int, double>> graph in Graphs)
            {
                RunTest(graph.Key, graph.Value, ALGORITHM.KRUSKAL);
                RunTest(graph.Key, graph.Value, ALGORITHM.PRIM);
            }
            Console.WriteLine("Done running minimum spanning tree tests.");
        }

        private void ReadGraphsFromFile()
        {
            Console.WriteLine("Reading graphs from file...");
            foreach (string file in Files)
            {
                try
                {
                    if (!Graphs.ContainsKey(file))
                    {
                        Console.WriteLine($" - loading '{file}'...");
                        Graphs.Add(file, ReadGraphFromFile(file));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($" ! Error while parsing '{file}': {exception.Message}");
                }
            }
            Console.WriteLine("Done reading graphs from file.\n");
        }

        private void RunTest(string file, IWeightedGraph<int, double> graph, ALGORITHM algorithm)
        {
            Stopwatch sw = new Stopwatch();
            switch (algorithm)
            {
                case ALGORITHM.KRUSKAL:
                    sw.Start();
                    Algorithms.MinimumSpanningTree.Kruskal.FindMinimumSpanningTree(graph);
                    sw.Stop();
                    break;
                case ALGORITHM.PRIM:
                    sw.Start();
                    Algorithms.MinimumSpanningTree.Prim.FindMinimumSpanningTree(graph, 0, double.MaxValue);
                    sw.Stop();
                    break;
            }

            Console.WriteLine($" + Computed minimum spanning tree of '{file}' using {algorithm.ToString()}'s algorithm in {sw.Elapsed}.");
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string fileName)
        {
            return Factory.CreateGraphFromFile($"./graphs/{fileName}", Parser, false);
        }
    }

    internal enum ALGORITHM
    {
        KRUSKAL,
        PRIM
    }
}
