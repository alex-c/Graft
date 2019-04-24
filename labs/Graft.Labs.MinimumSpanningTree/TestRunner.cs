using Graft.Default;
using Graft.Default.File;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Serilog;
using Graft.Labs.Utilities;

namespace Graft.Labs.MinimumSpanningTree
{
    public class TestRunner
    {
        private const string PATH = "./graphs/";

        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        private HashSet<string> Files { get; }

        private Dictionary<string, IWeightedGraph<int, double>> Graphs { get; }

        private HashSet<ALGORITHM> AlgorithmsToTest { get; }

        private Microsoft.Extensions.Logging.ILogger Logger { get; }

        public TestRunner()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
            Files = new HashSet<string>();
            Graphs = new Dictionary<string, IWeightedGraph<int, double>>();

            // Set which algorithms should be tested
            AlgorithmsToTest = new HashSet<ALGORITHM>()
            {
                ALGORITHM.KRUSKAL,
                ALGORITHM.PRIM
            };

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            // Set Serilog logger to Microsoft's ILogger
            Logger = new LoggerFactory().AddSerilog().CreateLogger<TestRunner>();
        }

        public void AddFile(string file)
        {
            Files.Add(file);
        }

        public void RunTests()
        {
            // Read graphs to test from files
            ReadGraphsFromFile();

            // Prepare table printing
            PrettyPrinter printer = new PrettyPrinter(Logger);
            TableBuilder table = printer.BuildTable(new string[] { "Graph", "Algorithm", "Time" }, "Execution Times");

            // Perform tests
            Logger.LogInformation("Run minimum spanning tree tests...");
            foreach (KeyValuePair<string, IWeightedGraph<int, double>> graph in Graphs)
            {
                foreach (ALGORITHM algorithm in AlgorithmsToTest)
                {
                    TimeSpan time = RunTest(graph.Key, graph.Value, algorithm);
                    table.AddLine(new string[] { graph.Key, algorithm.ToString(), time.ToString() });
                }
            }
            Logger.LogInformation("Done running minimum spanning tree tests.\n");

            // Display times as table
            table.Print();
        }

        private void ReadGraphsFromFile()
        {
            Logger.LogInformation("Reading graphs from file...");
            foreach (string file in Files)
            {
                try
                {
                    if (!Graphs.ContainsKey(file))
                    {
                        Logger.LogDebug($" - loading '{file}'...");
                        Graphs.Add(file, ReadGraphFromFile(file));
                    }
                }
                catch (Exception exception)
                {
                    Logger.LogWarning($" ! Error while parsing '{file}': {exception.Message}");
                }
            }
            Logger.LogInformation("Done reading graphs from file.\n");
        }

        private TimeSpan RunTest(string file, IWeightedGraph<int, double> graph, ALGORITHM algorithm)
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

            Logger.LogDebug($" - Computed MST for '{file}' using {algorithm}'s algorithm in {sw.Elapsed}.");

            return sw.Elapsed;
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
