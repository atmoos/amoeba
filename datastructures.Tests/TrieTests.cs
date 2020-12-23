using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace datastructures.Tests
{
    public class TrieTests
    {
        [Fact]
        public void TrieContainsAllWords()
        {
            var words = new[] { "hello", "ell", "world", "old", "lord" };
            var trie = new Trie<Char> { words };

            foreach(var word in words) {
                Assert.True(trie.Contains(word));
            }
        }

        [Fact]
        public void TrieContainsAllPrefixes()
        {
            var trie = new Trie<Char> { "Gattaca" };
            var prefixes = new[] { "G", "Gat", "Gatt", "Gatta", "Gattac", "Gattaca" };

            foreach(var prefix in prefixes) {
                Assert.True(trie.Contains(prefix));
            }
        }

        [Fact]
        public void TrieDoesNotContainAnySuffixes()
        {
            var trie = new Trie<Char> { "Gattaca" };
            Assert.False(trie.Contains("a"));
        }
    }
    public class TrieStorageTests
    {
        [Fact]
        public void TrieContainsAllWords()
        {
            var words = new[] { "hello", "ell", "world", "old", "lord" };
            var trie = new Trie<Char, String> { Lookup(words) };

            foreach(var word in words) {
                Assert.True(trie.TryGetValue(word, out var value));
                Assert.Equal(word, value);
            }
        }

        [Fact]
        public void TrieHasNoValuesStoredForAnyPrefix()
        {
            var trie = new Trie<Char, String> { Lookup("Gattaca") };
            var prefixes = new[] { "G", "Gat", "Gatt", "Gatta", "Gattac" };

            foreach(var prefix in prefixes) {
                Assert.False(trie.TryGetValue(prefix, out var value));
                Assert.Null(value);
            }
        }

        [Fact]
        public void TrieDoesNotContainAnySuffixes()
        {
            var trie = new Trie<Char, String> { Lookup("Gattaca") };
            Assert.False(trie.TryGetValue("a", out var _));
        }
        private static IEnumerable<(String, String)> Lookup(params String[] words) => words.Select(w => (w, w));
    }
}
