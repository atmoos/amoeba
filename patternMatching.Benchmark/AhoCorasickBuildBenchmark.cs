using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using NReco.Text;

namespace patternMatching.Benchmark;

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

    [Benchmark]
    public ISearch<Char, String> AhoCorasickFullBuild() => new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();

    [Benchmark]
    public AhoCorasickDoubleArrayTrie<String> DoubleTrieFullBuild() => new AhoCorasickDoubleArrayTrie<String>(this.doubleTrieDict);
}
