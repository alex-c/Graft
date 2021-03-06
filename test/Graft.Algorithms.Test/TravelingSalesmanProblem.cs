﻿using Graft.Algorithms.TravelingSalesmanProblem;
using Graft.Default;
using Graft.Default.File;
using Graft.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class TravelingSalesmanProblem
    {
        private GraphFactory<int, double> Factory { get; }

        public TravelingSalesmanProblem()
        {
            Factory = new GraphFactory<int, double>();
        }

        [TestMethod]
        [ExpectedException(typeof(GraphNotCompleteException))]
        public void TestNearestNerighborFailOnIncompleteGraph()
        {
            Graph<int, double> graph = new GraphBuilder<int, double>()
                .AddVerteces(5, v => v)
                .AddEdge(0, 2)
                .AddEdge(2, 1)
                .AddEdge(2, 3)
                .AddEdge(2, 4)
                .Build();

            NearestNeighbor.FindTour(graph);
        }

        [TestMethod]
        [ExpectedException(typeof(GraphNotCompleteException))]
        public void TestDoubleTreeFailOnIncompleteGraph()
        {
            Graph<int, double> graph = new GraphBuilder<int, double>()
                .AddVerteces(5, v => v)
                .AddEdge(0, 2)
                .AddEdge(2, 1)
                .AddEdge(2, 3)
                .AddEdge(2, 4)
                .Build();

            NearestNeighbor.FindTour(graph);
        }

        [TestMethod]
        public void TestTinyGraphs()
        {
            DefaultGraphTextLineParser parser = new DefaultGraphTextLineParser();
            Graph<int, double> graph1 = Factory.CreateGraphFromFile("./graphs/complete/K_10.txt", parser);
            Graph<int, double> graph2 = Factory.CreateGraphFromFile("./graphs/complete/K_10e.txt", parser);
            TestTspAlgorithms(graph1, 10, 10, 38.41);
            TestTspAlgorithms(graph2, 10, 10, 27.26);
        }

        [TestMethod]
        public void TestSmallGraphs()
        {
            DefaultGraphTextLineParser parser = new DefaultGraphTextLineParser();
            Graph<int, double> graph1 = Factory.CreateGraphFromFile("./graphs/complete/K_12.txt", parser);
            Graph<int, double> graph2 = Factory.CreateGraphFromFile("./graphs/complete/K_12e.txt", parser);
            TestTspAlgorithms(graph1, 12, 12, 45.19);
            TestTspAlgorithms(graph2, 12, 12, 36.13);
        }

        private void TestTspAlgorithms(IWeightedGraph<int, double> graph,
            int expectedVerteces,
            int expectedEdges,
            double optimalTourCosts = 38.41,
            double precision = 0.01)
        {
            TestTspAlgorithm(graph, TspAlgorithm.NearestNeighbor, expectedVerteces, expectedEdges, optimalTourCosts);
            TestTspAlgorithm(graph, TspAlgorithm.DoubleTree, expectedVerteces, expectedEdges, optimalTourCosts);
            TestTspAlgorithm(graph, TspAlgorithm.BruteForce, expectedVerteces, expectedEdges, optimalTourCosts);
        }

        private void TestTspAlgorithm(IWeightedGraph<int, double> graph,
            TspAlgorithm algorithm,
            int expectedVerteces,
            int expectedEdges,
            double optimalTourCosts = 0.0,
            double precision = 0.01)
        {
            IWeightedGraph<int, double> tour = null;

            // Compute tour with the chosen algorithm
            switch (algorithm)
            {
                case TspAlgorithm.NearestNeighbor:
                    tour = NearestNeighbor.FindTour(graph);
                    break;
                case TspAlgorithm.DoubleTree:
                    tour = DoubleTree.FindTour(graph);
                    break;
                case TspAlgorithm.BruteForce:
                    tour = BruteForce.FindOptimalTour(graph, 0.0, double.MaxValue, (w1, w2) => w1 + w2);
                    break;
                default:
                    throw new NotSupportedException($"Testing TSP with the {algorithm} algorithm is currently not supported.");
            }

            // Check route for component count
            Assert.AreEqual(expectedVerteces, tour.VertexCount);
            Assert.AreEqual(expectedEdges, tour.GetAllEdges().Count());

            // For algorithms that find the optimal tour, check the optmial tour costs
            if (algorithm == TspAlgorithm.BruteForce)
            {
                AssertDoublesNearlyEqual(optimalTourCosts, tour.GetAllEdges().Sum(e => e.Weight), precision);
            }
        }

        private void AssertDoublesNearlyEqual(double expected, double actual, double precision)
        {
            if (actual < expected - precision || actual > expected + precision)
            {
                throw new AssertFailedException($"Doubles are not equal with a precision of {precision}. Expected {expected}, got {actual}.");
            }
        }

        internal enum TspAlgorithm
        {
            NearestNeighbor,
            DoubleTree,
            BruteForce
        }
    }
}
