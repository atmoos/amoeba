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
