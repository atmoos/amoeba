using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using NReco.Text;

namespace patternMatching.Benchmark;

[ShortRunJob]
[IterationCount(7)]
[MemoryDiagnoser]
public class AhoCorasickBuildBenchmark
{
    [Params(60, 180)]
    public Int32 WordsInDictionary { get; set; }

    [Params(12, 36)]
    public Int32 WordSize { get; set; }
    private DataSource source;
    private ISearchBuilder<Char, String> aho;
    private AhoCorasickDoubleArrayTrie<String> doubleTrie;
    private KeyValuePair<String, String>[] doubleTrieDict;

    [GlobalSetup]
    public void Setup()
    {
        this.source = new DataSource(WordSize, WordsInDictionary);
        this.aho = new AhoCorasick<Char, String>() { this.source.Dictionary };
        this.doubleTrie = new AhoCorasickDoubleArrayTrie<String>();
        this.doubleTrieDict = this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)).ToArray();
    }

    [Benchmark]
    public ISearch<Char, String> AhoCorasickBuild() => this.aho.Build();

    [Benchmark(Baseline = true)]
    public AhoCorasickDoubleArrayTrie<String> DoubleTrieBuild()
    {
        this.doubleTrie.Build(this.doubleTrieDict);
        return this.doubleTrie;
    }
}

/*
// * Summary *

BenchmarkDotNet=v0.13.2, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2
  Job-AIPBYH : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2

IterationCount=7  LaunchCount=1  WarmupCount=3  

|           Method | WordsInDictionary | WordSize |        Mean |     Error |    StdDev | Ratio | RatioSD |      Gen0 |      Gen1 |     Gen2 |   Allocated | Alloc Ratio |
|----------------- |------------------ |--------- |------------:|----------:|----------:|------:|--------:|----------:|----------:|---------:|------------:|------------:|
| AhoCorasickBuild |                60 |       12 |    414.6 us |   5.80 us |   2.57 us |  0.26 |    0.01 |  100.0977 |   53.7109 |        - |   493.98 KB |        0.03 |
|  DoubleTrieBuild |                60 |       12 |  1,611.5 us | 140.76 us |  62.50 us |  1.00 |    0.00 | 1064.4531 | 1052.7344 | 994.1406 | 19226.59 KB |        1.00 |
|                  |                   |          |             |           |           |       |         |           |           |          |             |             |
| AhoCorasickBuild |                60 |       36 |  2,068.5 us |  21.81 us |   7.78 us |  0.78 |    0.02 |  320.3125 |  316.4063 |        - |  1975.42 KB |        0.10 |
|  DoubleTrieBuild |                60 |       36 |  2,657.3 us | 201.89 us |  72.00 us |  1.00 |    0.00 | 1164.0625 | 1117.1875 | 988.2813 | 19920.26 KB |        1.00 |
|                  |                   |          |             |           |           |       |         |           |           |          |             |             |
| AhoCorasickBuild |               180 |       12 |  1,257.9 us |  22.33 us |   9.91 us |  0.51 |    0.03 |  236.3281 |  234.3750 |        - |  1458.04 KB |        0.07 |
|  DoubleTrieBuild |               180 |       12 |  2,464.7 us | 262.98 us | 116.77 us |  1.00 |    0.00 | 1140.6250 | 1109.3750 | 988.2813 | 19779.17 KB |        1.00 |
|                  |                   |          |             |           |           |       |         |           |           |          |             |             |
| AhoCorasickBuild |               180 |       36 |  9,079.6 us | 489.98 us | 217.55 us |  0.52 |    0.01 | 1015.6250 |  437.5000 | 140.6250 |  5600.83 KB |        0.26 |
|  DoubleTrieBuild |               180 |       36 | 17,297.3 us | 237.54 us | 105.47 us |  1.00 |    0.00 | 1375.0000 | 1312.5000 | 906.2500 |  21730.8 KB |        1.00 |
*/