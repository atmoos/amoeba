using BenchmarkDotNet.Running;
using Data.Structures.Benchmark;

namespace datastructures.benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TrieMemoryFootprint>();
        }
    }
}