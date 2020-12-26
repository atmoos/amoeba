using BenchmarkDotNet.Running;

namespace patternMatching.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<AhoCorasickSearchBenchmark>();
        }
    }
}