﻿using System;
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
| AhoCorasickSearch |                60 |           800 |       12 | 162.3 us | 1.76 us | 1.64 us |  1.16 |    0.01 |     296 B |        0.80 |
|   DoubleTrieParse |                60 |           800 |       12 | 139.8 us | 0.54 us | 0.45 us |  1.00 |    0.00 |     368 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |                60 |           800 |       36 | 396.1 us | 6.36 us | 5.95 us |  1.02 |    0.02 |     152 B |        4.75 |
|   DoubleTrieParse |                60 |           800 |       36 | 387.0 us | 0.74 us | 0.69 us |  1.00 |    0.00 |      32 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |               120 |           800 |       12 | 151.4 us | 0.69 us | 0.65 us |  1.10 |    0.01 |     448 B |        0.58 |
|   DoubleTrieParse |               120 |           800 |       12 | 137.0 us | 1.42 us | 1.33 us |  1.00 |    0.00 |     776 B |        1.00 |
|                   |                   |               |          |          |         |         |       |         |           |             |
| AhoCorasickSearch |               120 |           800 |       36 | 416.8 us | 1.25 us | 1.17 us |  1.07 |    0.01 |     152 B |        4.75 |
|   DoubleTrieParse |               120 |           800 |       36 | 388.1 us | 1.91 us | 1.69 us |  1.00 |    0.00 |      32 B |        1.00 |
*/