using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace patternMatching.Benchmark
{
    public class AhoCorasickBenchmark
    {

        [Params(80, 200)]
        public Int32 WordsInDictionary { get; set; }

        [Params(200, 4000)]
        public Int32 TextWordCount { get; set; }

        [Params(12, 48)]
        public Int32 WordSize { get; set; }

        private DataSource source;
        private ISearch<Char, String> search;

        [GlobalSetup]
        public void Setup()
        {
            this.source = new DataSource(WordSize, WordsInDictionary);
            this.search = new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();
        }

        [Benchmark]
        public ISearch<Char, String> Building() => new AhoCorasick<Char, String>() { source.Dictionary }.Build();

        [Benchmark]
        public List<String> Searching() => this.search.Search(this.source.Text(TextWordCount)).ToList();
    }
}
