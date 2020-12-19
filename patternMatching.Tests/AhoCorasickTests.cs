using System;
using System.Linq;
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
        public void TrieMatchesOneOccurrence()
        {
            var trie = DefaultTrie();

            var match = trie.Search("halitops");

            Assert.Equal(Result("i"), match);
        }

        [Fact]
        public void TrieMatchesTwoOccurrencesOnlyOnce()
        {
            var trie = DefaultTrie();

            var match = trie.Search("halligalli");

            Assert.Equal(Result("i"), match);
        }

        [Fact]
        public void TrieMatchesTwoSeparatedPatternsDistinctly()
        {
            var trie = Trie("lee", "luv");

            var match = trie.Search("von luv ins lee");

            Assert.Equal(Result("luv", "lee"), match);
        }

        [Fact]
        public void TrieMatchesAdjacentSuffixPatternsDistinctly()
        {
            var trie = Trie("lee", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("lee", "leeward"), match);
        }

        [Fact]
        public void TrieMatchesOverlappingSuffixPatternsDistinctly()
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

        private static ITrie<Char, String> DefaultTrie() => Trie("i", "in", "tin", "sting");
        private static ITrie<Char, String> Trie(params String[] patterns)
        {
            var builder = new AhoCorasick();
            foreach(var pattern in patterns) {
                builder.Add(pattern);
            }
            return builder.Build();
        }

        private static IEnumerable<String> Result(params String[] result) => result;
    }
}
