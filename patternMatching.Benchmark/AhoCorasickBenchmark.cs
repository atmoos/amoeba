using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using BenchmarkDotNet;
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

        private sealed class DataSource
        {
            private readonly String[] textSegment;

            public IEnumerable<String> Dictionary { get; }

            public DataSource(Int32 wordSize, Int32 dictionarySize)
            {
                var rand = new Random(42);
                Dictionary = BuildWords(rand, wordSize, dictionarySize);
                this.textSegment = BuildWords(rand, wordSize, 256).ToArray();
            }

            public IEnumerable<Char> Text(Int32 wordCount) => InfiniteWords().Take(wordCount).SelectMany(s => s);
            private IEnumerable<String> InfiniteWords()
            {
                while(true) {
                    foreach(var letter in this.textSegment) {
                        yield return letter;
                    }
                }
            }
            private static List<String> BuildWords(Random rand, Int32 wordSize, Int32 wordCount)
            {
                var words = new List<String>(wordCount);
                var minWordSize = Math.Max(10, wordSize / 10);
                while(words.Count < wordCount) {
                    var index = 0;
                    var text = rand.Next().ToString();
                    var builder = new StringBuilder(rand.Next(minWordSize, wordSize));
                    while(builder.Length < builder.Capacity) {
                        if(index >= text.Length) {
                            text = rand.Next().ToString();
                            index = 0;
                        }
                        builder.Append(text[index]);
                    }
                    words.Add(builder.ToString());
                }
                return words;
            }
        }
    }
}
