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
            var trie = Trie("One", "two");

            var match = trie.Search("seven eight ten");

            Assert.Equal(Enumerable.Empty<string>(), match);
        }

        [Fact]
        public void TrieMatchesOneOccurrenceInTheCenter()
        {
            var trie = Trie("ab");

            var match = trie.Search("The ablative");

            Assert.Equal(Result("ab"), match);
        }

        [Fact]
        public void TrieMatchesOneOccurrenceAtTheEnd()
        {
            var trie = Trie("act");

            var match = trie.Search("The abstract");

            Assert.Equal(Result("act"), match);
        }

        [Fact]
        public void TrieMatchesEachOccurrenceOnce()
        {
            var trie = Trie("Fliegen");

            var match = trie.Search("Wenn Fliegen hinter Fliegen fliegen, fliegen Fliegen Fliegen nach!");

            Assert.Equal(Result("Fliegen", "Fliegen", "Fliegen", "Fliegen"), match);
        }
        [Fact]
        public void TrieMatchesTwoSeparatedPatterns()
        {
            var trie = Trie("lee", "luv");

            var match = trie.Search("von luv ins lee");

            Assert.Equal(Result("luv", "lee"), match);
        }

        [Fact]
        public void TrieMatchesAdjacentSuffixPatterns()
        {
            var trie = Trie("lee", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("lee", "leeward"), match);
        }

        [Fact]
        public void TrieMatchesAdjacentPostfixPatterns()
        {
            var trie = Trie("ward", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("leeward", "ward"), match);
        }

        [Fact]
        public void TrieMatchesOverlappingSuffixPatterns()
        {
            var trie = Trie("ton", "pontons");

            var match = trie.Search("The pontons!");

            Assert.Equal(Result("ton", "pontons"), match);
        }

        [Fact]
        public void TrieMatchesAreReturnedInTheOrderTheyAppearInTheSearchText()
        {
            var trie = Trie("seven", "five", "eight", "six");

            var match = trie.Search("... five, six, seven and eight!");

            Assert.Equal(Result("five", "six", "seven", "eight"), match);
        }

        [Fact]
        public void InterleavedPatternsAreResolved()
        {
            var trie = Trie("in", "tin", "sting");

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

        private static ITrie<Char, String> Trie(params String[] patterns)
        {
            var builder = new AhoCorasick();
            foreach(var pattern in patterns) {
                builder.Add(pattern);
            }
            return builder.Build();
        }

        private static IEnumerable<String> Result(params String[] result) => result;

        private static (TimeSpan naive, TimeSpan aho) TimeMatching(Int32 dictSize, Int32 textSize)
        {
            var rand = new Random(dictSize);
            var text = CreateText(rand, textSize);
            var dictionary = Enumerable.Range(0, dictSize).Select(_ => rand.Next(dictSize, textSize).ToString()).ToArray();
            var trie = Trie(dictionary);

            var timer = Stopwatch.StartNew();
            var trieMatches = trie.Search(text).ToHashSet();
            var trieTiming = timer.Elapsed;
            Console.WriteLine($"Trie: {trieTiming:g}");

            // naive
            timer.Restart();
            var naiveMatches = new HashSet<String>();
            foreach(var pattern in dictionary) {
                var match = new HashSet<StringBuilder>(pattern.Length);
                foreach(var letter in text) {
                    StringBuilder remove = null;
                    foreach(var m in match) {
                        m.Append(letter);
                        if(m.Length >= pattern.Length) {
                            if(m.ToString() == pattern) {
                                naiveMatches.Add(pattern);
                            }
                            remove = m;
                        }
                    }
                    match.Remove(remove);
                    match.Add(new StringBuilder(letter));
                }
            }
            var naiveTiming = timer.Elapsed;
            Console.WriteLine($"Naive: {naiveTiming:g}");
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
