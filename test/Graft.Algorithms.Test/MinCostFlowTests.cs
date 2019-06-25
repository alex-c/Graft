using Graft.Algorithms.MinCostFlow;
using Graft.BalanceGraph;
using Graft.Default;
using Graft.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class MinCostFlowTests
    {
        private BalanceGraphFactory Factory { get; }

        public MinCostFlowTests()
        {
            Factory = new BalanceGraphFactory();
        }

        [TestMethod]
        public void TestCycleCancelingForSmallGraphs()
        {
            TestSmallGraphs(Algorithm.CycleCanceling);
        }

        [TestMethod]
        public void TestSuccessiveShortestPathForSmallGraphs()
        {
            TestSmallGraphs(Algorithm.SuccessiveShortestPath);
        }

        private void TestSmallGraphs(Algorithm algorithm)
        {
            Graph<int, double> graph = LoadGraph("Kostenminimal1");
            IWeightedGraph<int, double> flow = FindCostMinimalFlow(graph, algorithm);
            Assert.AreEqual(3.0, ComputeCosts(flow));

            graph = LoadGraph("Kostenminimal4");
            flow = FindCostMinimalFlow(graph, algorithm);
            Assert.AreEqual(0.0, ComputeCosts(flow));

            graph = LoadGraph("Kostenminimal5");
            flow = FindCostMinimalFlow(graph, algorithm);
            Assert.AreEqual(0.0, ComputeCosts(flow));
        }

        [TestMethod]
        [ExpectedException(typeof(NoBFlowException))]
        public void TestCycleCancelingForFailingSmallGraph()
        {
            Graph<int, double> graph = LoadGraph("Kostenminimal2");
            FindCostMinimalFlow(graph, Algorithm.CycleCanceling);
        }

        [TestMethod]
        [ExpectedException(typeof(NoBFlowException))]
        public void TestSuccessiveShortestPathForFailingSmallGraph()
        {
            Graph<int, double> graph = LoadGraph("Kostenminimal2");
            FindCostMinimalFlow(graph, Algorithm.SuccessiveShortestPath);
        }

        [TestMethod]
        public void TestCycleCancelingForBigGraph()
        {
            Graph<int, double> graph = LoadGraph("Kostenminimal3");
            IWeightedGraph<int, double> flow = FindCostMinimalFlow(graph, Algorithm.CycleCanceling);
            Assert.AreEqual(1537.0, ComputeCosts(flow));
        }

        [TestMethod]
        public void TestSuccessiveShortestPathForBigGraph()
        {
            Graph<int, double> graph = LoadGraph("Kostenminimal3");
            IWeightedGraph<int, double> flow = FindCostMinimalFlow(graph, Algorithm.SuccessiveShortestPath);
            Assert.AreEqual(1537.0, ComputeCosts(flow));
        }

        private IWeightedGraph<int, double> FindCostMinimalFlow(Graph<int, double> graph, Algorithm algorithm)
        {
            IWeightedGraph<int, double> flow = null;
            int vertexCount = graph.GetAllVerteces().Count();
            switch (algorithm)
            {
                case Algorithm.CycleCanceling:
                    flow = CycleCanceling.FindCostMinimalFlow(graph,
                        vertexCount,
                        vertexCount + 1,
                        (v1, v2) => v1 + v2,
                        (v1, v2) => v1 - v2,
                        v => v * -1,
                        0.0,
                        double.MaxValue);
                    break;
                case Algorithm.SuccessiveShortestPath:
                    flow = SuccessiveShortestPath.FindCostMinimalFlow(graph,
                        vertexCount,
                        vertexCount + 1,
                        (v1, v2) => v1 + v2,
                        (v1, v2) => v1 - v2,
                        v => v * -1,
                        0.0,
                        double.MaxValue);
                    break;
                default:
                    throw new NotSupportedException($"No tests available for minimum cost flows algorithm '{algorithm}'");
            }
            return flow;
        }

        private Graph<int, double> LoadGraph(string file)
        {
            return Factory.CreateGraphFromFile($"./graphs/min_cost_flow/{file}.txt");
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

        private enum Algorithm
        {
            CycleCanceling,
            SuccessiveShortestPath
        }
    }
}
