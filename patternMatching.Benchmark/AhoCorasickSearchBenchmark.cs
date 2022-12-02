using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark;

[MemoryDiagnoser]
public class AhoCorasickSearchBenchmark
{

    [Params(60, 120)]
    public Int32 WordsInDictionary { get; set; }

    [Params(800)]
    public Int32 TextWordCount { get; set; }

    [Params(12, 36)]
    public Int32 WordSize { get; set; }

    private String text;
    private DataSource source;
    private ISearch<Char, String> aho;
    private AhoCorasickDoubleArrayTrie<String> doubleTrie;

    [GlobalSetup]
    public void Setup()
    {
        this.source = new DataSource(WordSize, WordsInDictionary);
        this.aho = new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();
        this.doubleTrie = new AhoCorasickDoubleArrayTrie<String>(this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)));
        this.text = source.TextAsString(TextWordCount);
    }

    [Benchmark]
    public List<String> AhoCorasickSearch() => this.aho.Search(this.text).ToList();

    [Benchmark(Baseline = true)]
    public Object DoubleTrieParse() => this.doubleTrie.ParseText(this.text);
}

/*
// * Summary *

BenchmarkDotNet=v0.13.2, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.56001), X64 RyuJIT AVX2


|            Method | WordsInDictionary | TextWordCount | WordSize |     Mean |   Error |  StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------ |------------------ |-------------- |--------- |---------:|--------:|--------:|------:|--------:|----------:|------------:|
| AhoCorasickSearch |                60 |           800 |       12 | 162.2 us | 1.75 us | 1.55 us |  1.16 |    0.02 |     528 B |        1.43 |
|   DoubleTrieParse |                60 |           800 |       12 | 140.2 us | 1.05 us | 0.98 us |  1.00 |    0.00 |     368 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |                60 |           800 |       36 | 390.2 us | 1.14 us | 1.01 us |  1.00 |    0.00 |     144 B |        4.50 |
|   DoubleTrieParse |                60 |           800 |       36 | 388.3 us | 1.58 us | 1.48 us |  1.00 |    0.00 |      32 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |               120 |           800 |       12 | 151.4 us | 0.47 us | 0.37 us |  1.08 |    0.01 |     840 B |        1.08 |
|   DoubleTrieParse |               120 |           800 |       12 | 139.7 us | 0.65 us | 0.61 us |  1.00 |    0.00 |     776 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |               120 |           800 |       36 | 417.4 us | 0.84 us | 0.79 us |  1.02 |    0.00 |     144 B |        4.50 |
|   DoubleTrieParse |               120 |           800 |       36 | 408.4 us | 0.98 us | 0.92 us |  1.00 |    0.00 |      32 B |        1.00 |
*/