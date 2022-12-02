using BenchmarkDotNet.Running;
using patternMatching.Benchmark;

var summary = BenchmarkRunner.Run<AhoCorasickBuildBenchmark>();
