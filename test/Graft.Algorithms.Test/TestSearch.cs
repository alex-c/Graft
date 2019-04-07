using Graft.Algorithms.Search;
using Graft.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Algorithms.Test
{
    [TestClass]
    public class TestSearch
    {
        [TestMethod]
        public void ConnectedComponents()
        {
            IGraph<int> graph = new Graph<int>();

            Assert.AreEqual(1, CountConnectedComponents(graph));
        }

        private int CountConnectedComponents(IGraph<int> graph)
        {
            int connectedComponents = 0;
            IVertex<int> nextUntaggedVertex = null;

            do
            {
                connectedComponents++;
                nextUntaggedVertex = graph.GetFirstMatchingVertex(v => !v.HasTag("component"));
                if (nextUntaggedVertex != null)
                {
                    BreadthFirstSearch.Search(graph, (v) => { v.AddTag("component", connectedComponents); });
                }
            }
            while (nextUntaggedVertex != null);

            return connectedComponents;
        }
    }
}
