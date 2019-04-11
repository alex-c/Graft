using Graft.Default;
using Graft.Default.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Graft.Labs.ConnectedComponents
{
    public class TestRunner
    {
        private const string PATH = "./graphs/";

        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        private HashSet<string> Files { get; }

        private Dictionary<string, IGraph<int>> Graphs { get; }

        public TestRunner()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
            Files = new HashSet<string>();
            Graphs = new Dictionary<string, IGraph<int>>();
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
            Console.WriteLine("Done loading graphs from file.");

            Console.WriteLine("Run connected components tests...");
            foreach (KeyValuePair<string, IGraph<int>> graph in Graphs)
            {
                RunTest(graph.Key, graph.Value);
            }
            Console.WriteLine("Done running connected components tests.");
        }

        private void RunTest(string file, IGraph<int> graph)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int cc = Algorithms.ConnectedComponents.Count(graph);
            sw.Stop();

            Console.WriteLine($" + Counted {cc} connected components of '{file}' in {sw.Elapsed}.");
        }

        private IGraph<int> ReadGraphFromFile(string fileName)
        {
            return Factory.CreateGraphFromFile($"./graphs/{fileName}", Parser, false);
        }
    }
}
