using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Xunit;

namespace patternMatching.Tests
{
    public class AhoCorasickTests
    {
        [Fact]
        public void NoMatchReturnsAnEmptyEnumerable()
        {
            var trie = SearchFor("One", "two");

            var match = trie.Search("seven eight ten");

            Assert.Equal(Enumerable.Empty<string>(), match);
        }

        [Fact]
        public void SearchMatchesOneOccurrenceInTheCenter()
        {
            var trie = SearchFor("ab");

            var match = trie.Search("The ablative");

            Assert.Equal(Result("ab"), match);
        }

        [Fact]
        public void SearchMatchesOneOccurrenceAtTheEnd()
        {
            var trie = SearchFor("act");

            var match = trie.Search("The abstract");

            Assert.Equal(Result("act"), match);
        }

        [Fact]
        public void SearchMatchesEachOccurrenceOnce()
        {
            var trie = SearchFor("Fliegen");

            var match = trie.Search("Wenn Fliegen hinter Fliegen fliegen, fliegen Fliegen Fliegen nach!");

            Assert.Equal(Result("Fliegen", "Fliegen", "Fliegen", "Fliegen"), match);
        }
        [Fact]
        public void SearchMatchesTwoSeparatedPatterns()
        {
            var trie = SearchFor("lee", "luv");

            var match = trie.Search("von luv ins lee");

            Assert.Equal(Result("luv", "lee"), match);
        }

        [Fact]
        public void SearchMatchesAdjacentSuffixPatterns()
        {
            var trie = SearchFor("lee", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("lee", "leeward"), match);
        }

        [Fact]
        public void SearchMatchesAdjacentPostfixPatterns()
        {
            var trie = SearchFor("ward", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("leeward", "ward"), match);
        }

        [Fact]
        public void SearchMatchesOverlappingSuffixPatterns()
        {
            var trie = SearchFor("ton", "pontons");

            var match = trie.Search("The pontons!");

            Assert.Equal(Result("ton", "pontons"), match);
        }

        [Fact]
        public void SearchMatchesAreReturnedInTheOrderTheyAppearInTheSearchText()
        {
            var trie = SearchFor("seven", "five", "eight", "six");

            var match = trie.Search("... five, six, seven and eight!");

            Assert.Equal(Result("five", "six", "seven", "eight"), match);
        }

        [Fact]
        public void InterleavedPatternsAreResolved()
        {
            var trie = SearchFor("in", "tin", "sting");

            var match = trie.Search("a tin in a stinger");

            Assert.Equal(Result("tin", "in", "in", "tin", "in", "sting"), match);
        }

        [Fact]
        public void AhoCorasickIsFasterThanNaiveApproach()
        {
            var warmup = TimeMatching(20, 800);
            var run = TimeMatching(40, 8000);
            Assert.True(run.naive > run.aho);
            Assert.NotEqual(warmup, run);
        }

        private static ISearch<Char, String> SearchFor(params String[] patterns) => new AhoCorasick<Char, String> { patterns }.Build();

        private static IEnumerable<String> Result(params String[] result) => result;

        private static (TimeSpan naive, TimeSpan aho) TimeMatching(Int32 dictSize, Int32 textSize)
        {
            var rand = new Random(dictSize);
            var text = CreateText(rand, textSize);
            var dictionary = Enumerable.Range(0, dictSize).Select(_ => rand.Next(dictSize, textSize).ToString()).ToArray();
            var trie = SearchFor(dictionary);
            var naive = new Naive.MultiPatternSearch<Char, String> { dictionary }.Build();

            var timer = Stopwatch.StartNew();
            var trieMatches = trie.Search(text).ToHashSet();
            var trieTiming = timer.Elapsed;
            Console.WriteLine($"Aho-C search: {trieTiming:g}");

            timer.Restart();
            var naiveMatches = naive.Search(text).ToHashSet();
            var naiveTiming = timer.Elapsed;
            Console.WriteLine($"Naive search: {naiveTiming:g}");
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
