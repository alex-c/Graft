using Graft.Default;
using Graft.Default.File;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Serilog;
using Graft.Labs.Utilities;
using System.Linq;

namespace Graft.Labs.TravelingSalesmanProblem
{
    public class TestRunner
    {
        private const string PATH = "./graphs/";

        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        private HashSet<string> Files { get; }

        private Dictionary<string, IWeightedGraph<int, double>> Graphs { get; }

        private HashSet<TspAlgorithm> AlgorithmsToTest { get; }

        private Microsoft.Extensions.Logging.ILogger Logger { get; }

        public TestRunner()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
            Files = new HashSet<string>();
            Graphs = new Dictionary<string, IWeightedGraph<int, double>>();

            // Set which algorithms should be tested
            AlgorithmsToTest = new HashSet<TspAlgorithm>()
            {
                TspAlgorithm.NearestNeighbor,
                TspAlgorithm.DoubleTree
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
            TableBuilder table = printer.BuildTable(new string[] { "Graph", "Algorithm", "Route Costs", "Time" }, "Traveling Salesman Problem");

            // Perform tests
            Logger.LogInformation("Run TSP tests...");
            foreach (KeyValuePair<string, IWeightedGraph<int, double>> graph in Graphs)
            {
                foreach (TspAlgorithm algorithm in AlgorithmsToTest)
                {
                    TestResult result = RunTest(graph.Key, graph.Value, algorithm);
                    table.AddLine(new string[] { graph.Key, algorithm.ToString(), result.Weight.ToString(), result.Time.ToString() });
                }
            }
            Logger.LogInformation("Done TSP tree tests.\n");

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

        private TestResult RunTest(string file, IWeightedGraph<int, double> graph, TspAlgorithm algorithm)
        {
            IWeightedGraph<int, double> route = null;
            Stopwatch sw = new Stopwatch();

            switch (algorithm)
            {
                case TspAlgorithm.NearestNeighbor:
                    sw.Start();
                    route = Algorithms.TravelingSalesmanProblem.NearestNeighbor.FindTour(graph);
                    sw.Stop();
                    break;
                case TspAlgorithm.DoubleTree:
                    sw.Start();
                    route = Algorithms.TravelingSalesmanProblem.DoubleTree.FindTour(graph);
                    sw.Stop();
                    break;
            }

            Logger.LogDebug($" - Computed route for '{file}' using {algorithm} algorithm in {sw.Elapsed}.");

            return new TestResult(route.GetAllEdges().Sum(e => e.Weight), sw.Elapsed);
        }

        private IWeightedGraph<int, double> ReadGraphFromFile(string fileName)
        {
            return Factory.CreateGraphFromFile($"./graphs/{fileName}", Parser, false);
        }
    }

    internal enum TspAlgorithm
    {
        NearestNeighbor,
        DoubleTree,
        BruteForce
    }

    internal class TestResult
    {
        public double Weight { get; }

        public TimeSpan Time { get; }

        public TestResult(double weight, TimeSpan time)
        {
            Weight = weight;
            Time = time;
        }
    }
}
