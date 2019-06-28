using Graft.Algorithms.MaxMatchings;
using Graft.Default;
using Graft.Default.File;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Tests
{
    [TestClass]
    public class MaxMatchingsTests
    {
        private GraphFactory<int, double> Factory { get; }

        private DefaultGraphTextLineParser Parser { get; }

        public MaxMatchingsTests()
        {
            Factory = new GraphFactory<int, double>();
            Parser = new DefaultGraphTextLineParser();
        }

        [TestMethod]
        public void TestGraph1()
        {
            IGraph<int> graph = LoadGraph("Matching_100_100");
            (HashSet<IVertex<int>> set1, HashSet<IVertex<int>> set2) = SplitGraphVerteces(graph, 100);
            IEnumerable<IEdge<int>> matching = BipartiteMatching.FindMaxMatching(graph, set1, set2, graph.VertexCount, graph.VertexCount + 1);
            Assert.AreEqual(100, matching.Count());
        }

        [TestMethod]
        public void TestGraph2()
        {
            IGraph<int> graph = LoadGraph("Matching2_100_100");
            (HashSet<IVertex<int>> set1, HashSet<IVertex<int>> set2) = SplitGraphVerteces(graph, 100);
            IEnumerable<IEdge<int>> matching = BipartiteMatching.FindMaxMatching(graph, set1, set2, graph.VertexCount, graph.VertexCount + 1);
            Assert.AreEqual(99, matching.Count());
        }

        private IGraph<int> LoadGraph(string graph)
        {
            return Factory.CreateGraphFromFile($"./graphs/matchings/{graph}.txt", Parser);
        }

        private (HashSet<IVertex<int>>, HashSet<IVertex<int>>) SplitGraphVerteces(IGraph<int> graph, int firstSetSize)
        {
            HashSet<IVertex<int>> set1 = new HashSet<IVertex<int>>();
            HashSet<IVertex<int>> set2 = new HashSet<IVertex<int>>();

            int i = 0;
            foreach (IVertex<int> vertex in graph.GetAllVerteces())
            {
                if (i < firstSetSize)
                {
                    set1.Add(vertex);
                }
                else
                {
                    set2.Add(vertex);
                }
                i++;
            }
            return (set1, set2);
        }
    }
}
