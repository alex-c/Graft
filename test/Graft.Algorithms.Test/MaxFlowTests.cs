using Graft.Algorithms.MaxFlow;
using Graft.Default;
using Graft.Default.File;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class MaxFlowTests
    {
        private GraphFactory<int, double> Factory { get; }
        private DefaultGraphTextLineParser Parser { get; }

        private Func<double, double, double> CombineFlowValues { get; }
        private Func<double, double, double> SubstractFlowValues { get; }

        public MaxFlowTests()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
            CombineFlowValues = (double currentFlow, double augmentingFlow) => currentFlow + augmentingFlow;
            SubstractFlowValues = (double currentFlow, double augmentingFlow) => currentFlow - augmentingFlow;
        }

        [TestMethod]
        public void TestTinyGraph()
        {
            IWeightedGraph<int, double> graph = LoadGraphFromFile("./graphs/max_flow/Fluss.txt");
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 0);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 7);
            IWeightedGraph<int, double> maxFlow = EdmondsKarp.FindMaxFlow(graph, source, target, CombineFlowValues, SubstractFlowValues, 0.0);
            double maxFlowValue = GetMaxFlowValue(maxFlow, source.Value);
            Assert.AreEqual(4, maxFlowValue);
        }

        [TestMethod]
        public void TestSmallGraph()
        {
            IWeightedGraph<int, double> graph = LoadGraphFromFile("./graphs/max_flow/Fluss2.txt");
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 0);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 7);
            IWeightedGraph<int, double> maxFlow = EdmondsKarp.FindMaxFlow(graph, source, target, CombineFlowValues, SubstractFlowValues, 0.0);
            double maxFlowValue = GetMaxFlowValue(maxFlow, source.Value);
            Assert.AreEqual(5, maxFlowValue);
        }

        [TestMethod]
        public void TestBigGraph()
        {
            IWeightedGraph<int, double> graph = LoadGraphFromFile("./graphs/weighted/G_1_2.txt");
            IVertex<int> source = graph.GetFirstMatchingVertex(v => v.Value == 0);
            IVertex<int> target = graph.GetFirstMatchingVertex(v => v.Value == 7);
            IWeightedGraph<int, double> maxFlow = EdmondsKarp.FindMaxFlow(graph, source, target, CombineFlowValues, SubstractFlowValues, 0.0);
            double maxFlowValue = GetMaxFlowValue(maxFlow, source.Value);
            AssertDoublesNearlyEqual(0.735802, maxFlowValue, 0.000001);
        }

        private IWeightedGraph<int, double> LoadGraphFromFile(string path)
        {
            return Factory.CreateGraphFromFile(path, Parser, true);
        }

        private void AssertDoublesNearlyEqual(double expected, double actual, double precision)
        {
            if (actual < expected - precision || actual > expected + precision)
            {
                throw new AssertFailedException($"Doubles are not equal with a precision of {precision}. Expected {expected}, got {actual}.");
            }
        }

        private double GetMaxFlowValue(IWeightedGraph<int, double> flow, int sourceValue)
        {
            IVertex<int> source = flow.GetFirstMatchingVertex(v => v.Value == sourceValue);
            return flow.GetEdgesOfVertex(source)
                .Where(e => ((IWeightedDirectedEdge<int, double>)e).OriginVertex == source)
                .Sum(e => e.Weight);
        }
    }
}
