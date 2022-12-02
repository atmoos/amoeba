using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark;

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class AhoCorasickFullBuildBenchmark
{

    [Params(60, 240)]
    public Int32 WordsInDictionary { get; set; }

    [Params(12, 24)]
    public Int32 WordSize { get; set; }
    private DataSource source;
    private KeyValuePair<String, String>[] doubleTrieDict;

    [GlobalSetup]
    public void Setup()
    {
        this.source = new DataSource(WordSize, WordsInDictionary);
        this.doubleTrieDict = this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)).ToArray();
    }

    [Benchmark]

    public ISearch<Char, String> NaiveFullBuild() => new Naive.MultiPatternSearch<Char, String> { this.source.Dictionary }.Build();

    [Benchmark]
    public ISearch<Char, String> AhoCorasickFullBuild() => new AhoCorasick<Char, String> { this.source.Dictionary }.Build();

    [Benchmark(Baseline = true)]
    public AhoCorasickDoubleArrayTrie<String> DoubleTrieFullBuild() => new AhoCorasickDoubleArrayTrie<String>(this.doubleTrieDict);
}

/*
// * Summary *

BenchmarkDotNet=v0.13.2, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2
  Job-FBQSMF : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2

IterationCount=7  LaunchCount=1  WarmupCount=3  

|               Method | WordsInDictionary | WordSize |        Mean |      Error |     StdDev | Ratio | RatioSD |      Gen0 |      Gen1 |     Gen2 |   Allocated | Alloc Ratio |
|--------------------- |------------------ |--------- |------------:|-----------:|-----------:|------:|--------:|----------:|----------:|---------:|------------:|------------:|
|       NaiveFullBuild |                60 |       12 |    30.75 us |   0.181 us |   0.064 us |  0.03 |    0.00 |    3.5095 |         - |        - |    14.41 KB |       0.001 |
| AhoCorasickFullBuild |                60 |       12 |   381.92 us |   5.180 us |   2.300 us |  0.32 |    0.03 |  113.7695 |   75.1953 |        - |    576.4 KB |       0.030 |
|  DoubleTrieFullBuild |                60 |       12 | 1,220.21 us | 276.059 us |  98.445 us |  1.00 |    0.00 | 1064.4531 |  994.1406 | 994.1406 | 19226.64 KB |       1.000 |
|                      |                   |          |             |            |            |       |         |           |           |          |             |             |
|       NaiveFullBuild |                60 |       24 |    33.98 us |   0.207 us |   0.074 us |  0.02 |    0.00 |    5.0049 |         - |        - |    20.48 KB |       0.001 |
| AhoCorasickFullBuild |                60 |       24 | 1,158.34 us |  26.184 us |  11.626 us |  0.57 |    0.02 |  234.3750 |  232.4219 |        - |  1447.84 KB |       0.074 |
|  DoubleTrieFullBuild |                60 |       24 | 2,025.25 us | 164.418 us |  73.003 us |  1.00 |    0.00 | 1144.5313 | 1132.8125 | 994.1406 |  19578.5 KB |       1.000 |
|                      |                   |          |             |            |            |       |         |           |           |          |             |             |
|       NaiveFullBuild |               240 |       12 |   325.85 us |   2.174 us |   0.775 us |  0.13 |    0.01 |   14.1602 |    0.4883 |        - |    58.34 KB |       0.003 |
| AhoCorasickFullBuild |               240 |       12 | 2,088.42 us |  21.822 us |   9.689 us |  0.82 |    0.04 |  371.0938 |  367.1875 |        - |   2293.6 KB |       0.114 |
|  DoubleTrieFullBuild |               240 |       12 | 2,542.97 us | 305.882 us | 135.814 us |  1.00 |    0.00 | 1187.5000 | 1183.5938 | 988.2813 | 20059.25 KB |       1.000 |
|                      |                   |          |             |            |            |       |         |           |           |          |             |             |
|       NaiveFullBuild |               240 |       24 |   320.78 us |   2.382 us |   1.058 us |  0.05 |    0.00 |   19.5313 |    0.9766 |        - |    81.59 KB |       0.004 |
| AhoCorasickFullBuild |               240 |       24 | 7,353.03 us | 168.985 us |  60.262 us |  1.18 |    0.02 |  921.8750 |  617.1875 | 328.1250 |  5663.61 KB |       0.264 |
|  DoubleTrieFullBuild |               240 |       24 | 6,233.77 us | 154.336 us |  68.526 us |  1.00 |    0.00 | 1390.6250 | 1375.0000 | 976.5625 | 21428.71 KB |       1.000 |
*/