using System;
using BenchmarkDotNet.Running;

namespace patternMatching.Benchmark;

public class Program
{
    public static void Main(String[] args)
    {
        var summary = BenchmarkRunner.Run<AhoCorasickSearchBenchmark>();
    }
}
