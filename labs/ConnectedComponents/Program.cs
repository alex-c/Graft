using System;

namespace Graft.Labs.ConnectedComponents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestRunner tr = new TestRunner();
            tr.AddFile("Graph1.txt");
            tr.AddFile("Graph2.txt");
            tr.AddFile("Graph3.txt");
            tr.AddFile("Graph_gross.txt");
            tr.AddFile("Graph_ganzgross.txt");
            tr.AddFile("Graph_ganzganzgross.txt");
            tr.RunTests();

            Console.ReadLine();
        }
    }
}
