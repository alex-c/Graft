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
            Console.WriteLine("Loading graphs from file...");
            foreach (string file in Files)
            {
                if (!Graphs.ContainsKey(file))
                {
                    Console.WriteLine($" + loading '{file}'...");
                    Graphs.Add(file, ReadGraphFromFile(file));
                }
            }
            Console.WriteLine("Done loading graphs from file.\n");

            Console.WriteLine("Run minimum spanning tree tests...");
            foreach (KeyValuePair<string, IWeightedGraph<int, double>> graph in Graphs)
            {
                RunTest(graph.Key, graph.Value);
            }
            Console.WriteLine("Done running minimum spanning tree tests.");
        }

        private void RunTest(string file, IWeightedGraph<int, double> graph)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Algorithms.MinimumSpanningTree.Kruskal.FindMinimumSpanningTree(graph);
            sw.Stop();

            Console.WriteLine($" + Computed minimum spanning tree of '{file}' using Kruskal's algorithm in {sw.Elapsed}.");
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string fileName)
        {
            return Factory.CreateGraphFromFile($"./graphs/{fileName}", Parser, false);
        }
    }
}
