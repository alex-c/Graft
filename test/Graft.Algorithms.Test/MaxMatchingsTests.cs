using Graft.Default;
using Graft.Default.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            IGraph<int> graph = LoadGraph("Matching_100_100.txt");
            // TODO: find matchings
            // TODO: assert we found 100 matching edges
        }

        [TestMethod]
        public void TestGraph2()
        {
            IGraph<int> graph = LoadGraph("Matching2_100_100.txt");
            // TODO: find matchings
            // TODO: assert we found 99 matching edges
        }

        private IGraph<int> LoadGraph(string graph)
        {
            return Factory.CreateGraphFromFile($"./graphs/matchings/{graph}.txt", Parser);
        }
    }
}
