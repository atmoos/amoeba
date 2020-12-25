using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xunit;

namespace patternMatching.Tests
{
    public sealed class AhoCorasickTests : TestSearchAlgorithm<AhoCorasick<Char, String>>
    {
        [Fact]
        public void OverlapingKeys()
        {
            // This is a minimal test!
            // I.e: All characters in the assert and act strings are relevant.
            var trie = SearchFor("16", "36");

            var match = trie.Search("136");

            Assert.Equal(Result("36"), match);
        }

        [Fact]
        public void AhoCorasickIsFasterThanNaiveApproach()
        {
            var warmup = TimeMatching(20, 800);
            var run = TimeMatching(40, 8000);
            Assert.True(run.naive > run.aho);
            Assert.NotEqual(warmup, run);
        }

        private static (TimeSpan naive, TimeSpan aho) TimeMatching(Int32 dictSize, Int32 textSize)
        {
            var rand = new Random(dictSize);
            var text = CreateText(rand, textSize);
            var dictionary = Enumerable.Range(0, dictSize).Select(_ => rand.Next(dictSize, textSize).ToString()).ToArray();
            var trie = new AhoCorasick<Char, String> { dictionary }.Build();
            var naive = new Naive.MultiPatternSearch<Char, String> { dictionary }.Build();

            var timer = Stopwatch.StartNew();
            var trieMatches = trie.Search(text).ToList();
            var trieTiming = timer.Elapsed;

            timer.Restart();
            var naiveMatches = naive.Search(text).ToList();
            var naiveTiming = timer.Elapsed;

            Assert.Equal(naiveMatches, trieMatches);
            return (naiveTiming, trieTiming);
        }

        private static String CreateText(Random rand, int inputSize)
        {
            const Int32 scale = 23;
            var text = new StringBuilder();
            var maxInteger = scale * inputSize;
            for(Int32 i = 0; i < inputSize; i++) {
                var next = rand.Next(10, maxInteger);
                text.Append(next);
                if(next % 7 == 0) {
                    text.Append(" ");
                    continue;
                }
                if(next % 53 == 0) {
                    text.Append(". ");
                }
            }
            return text.ToString();
        }
    }
}
