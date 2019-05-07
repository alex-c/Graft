using System;

namespace Graft.Labs.TravelingSalesmanProblem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestRunner tr = new TestRunner();
            tr.AddFile("K_10.txt");
            tr.AddFile("K_10e.txt");
            tr.AddFile("K_12.txt");
            tr.AddFile("K_12e.txt");
            tr.AddFile("K_15.txt");
            tr.AddFile("K_15e.txt");
            tr.AddFile("K_20.txt");
            tr.AddFile("K_30.txt");
            tr.AddFile("K_50.txt");
            tr.AddFile("K_70.txt");
            tr.AddFile("K_100.txt");
            tr.RunTests();

            Console.ReadLine();
        }
    }
}
