using System;

namespace Graft.Labs.MinimumSpanningTree
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestRunner tr = new TestRunner();
            tr.AddFile("G_1_2.txt");
            tr.AddFile("G_1_20.txt");
            tr.AddFile("G_1_200.txt");
            tr.AddFile("G_10_20.txt");
            tr.AddFile("G_10_200.txt");
            tr.AddFile("G_100_200.txt");
            tr.RunTests();

            Console.ReadLine();
        }
    }
}
