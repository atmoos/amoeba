using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace patternMatching.Benchmark
{
    public class AhoCorasickBenchmark
    {

        [Params(60)]
        public Int32 WordsInDictionary { get; set; }

        [Params(400)]
        public Int32 TextWordCount { get; set; }

        [Params(8)]
        public Int32 WordSize { get; set; }

        private DataSource source;
        private ISearch<Char, String> naive;
        private ISearch<Char, String> aho;

        [GlobalSetup]
        public void Setup()
        {
            this.source = new DataSource(WordSize, WordsInDictionary);
            this.aho = new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();
            this.naive = new Naive.MultiPatternSearch<Char, String> { this.source.Dictionary }.Build();
        }

        [Benchmark(Baseline = true)]

        public List<String> NaiveSearch() => this.naive.Search(this.source.Text(TextWordCount / 50)).ToList();

        [Benchmark]
        public ISearch<Char, String> Building() => new AhoCorasick<Char, String>() { source.Dictionary }.Build();

        [Benchmark]
        public List<String> Searching() => this.aho.Search(this.source.Text(TextWordCount)).ToList();
    }
}
