using Graft.Algorithms.MinCostFlow;
using Graft.BalanceGraph;
using Graft.Default;
using Graft.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class MinCostFlowTests
    {
        private static readonly string[] Files = { "Kostenminimal1", "Kostenminimal2", "Kostenminimal3", "Kostenminimal4", "Kostenminimal5" };

        private Dictionary<string, Graph<int, double>> Graphs { get; }

        public MinCostFlowTests()
        {
            Graphs = new Dictionary<string, Graph<int, double>>();
            BalanceGraphFactory factory = new BalanceGraphFactory();
            foreach (string file in Files)
            {
                Graphs.Add(file, factory.CreateGraphFromFile($"./graphs/min_cost_flow/{file}.txt"));
            }
        }

        [TestMethod]
        public void TestSmallGraphs()
        {
            Graph<int, double> graph = Graphs["Kostenminimal1"];
            IWeightedGraph<int, double> flow = FindCostMinimalFlow(graph);
            Assert.AreEqual(3.0, ComputeCosts(flow));

            graph = Graphs["Kostenminimal4"];
            flow = FindCostMinimalFlow(graph);
            Assert.AreEqual(0.0, ComputeCosts(flow));

            graph = Graphs["Kostenminimal5"];
            flow = FindCostMinimalFlow(graph);
            Assert.AreEqual(0.0, ComputeCosts(flow));
        }

        [TestMethod]
        [ExpectedException(typeof(NoBFlowException))]
        public void TestFailingSmallGraph()
        {
            Graph<int, double> graph = Graphs["Kostenminimal2"];
            FindCostMinimalFlow(graph);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            Graph<int, double> graph = Graphs["Kostenminimal3"];
            IWeightedGraph<int, double> flow = FindCostMinimalFlow(graph);
            Assert.AreEqual(1537.0, ComputeCosts(flow));
        }

        private IWeightedGraph<int, double> FindCostMinimalFlow(Graph<int, double> graph)
        {
            int vertexCount = graph.GetAllVerteces().Count();
            return CycleCanceling.FindCostMinimalFlow(graph,
                vertexCount,
                vertexCount + 1,
                (v1, v2) => v1 + v2,
                (v1, v2) => v1 - v2,
                v => v * -1,
                0.0,
                double.MaxValue);
        }

        private double ComputeCosts(IWeightedGraph<int, double> graph)
        {
            double costs = 0.0;
            foreach (Primitives.IWeightedEdge<int, double> edge in graph.GetAllEdges())
            {
                costs += edge.GetAttribute<double>(Constants.FLOW) * edge.GetAttribute<double>(Constants.COSTS);
            }
            return costs;
        }
    }
}
