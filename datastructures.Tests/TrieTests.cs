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
            var words = Words("hello", "ell", "world", "old", "lord");
            var trie = new Trie<Char> { words };

            foreach(var word in words) {
                Assert.True(trie.Contains(word));
            }
        }

        [Fact]
        public void TrieContainsAllPrefixes()
        {
            var trie = new Trie<Char> { "Gattaca" };
            var prefixes = Words("G", "Gat", "Gatt", "Gatta", "Gattac", "Gattaca");

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
        private static String[] Words(params String[] words) => words;
    }
    public class TrieStorageTests
    {
        [Fact]
        public void TrieContainsAllWords()
        {
            var words = Words("hello", "ell", "world", "old", "lord");
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
            var prefixes = Words("G", "Gat", "Gatt", "Gatta", "Gattac");

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
        [Fact]
        public void PrefixBundlingDoesNotCauseKeyCollisions()
        {
            var words = Words("two", "one-two", "wo"); // "wo", being the common suffix
            var trie = new Trie<Char, String> { Lookup(words) };

            foreach(var word in words) {
                Assert.True(trie.TryGetValue(word, out var value));
                Assert.Equal(word, value);
            }
        }
        [Fact]
        public void SuffixBundlingDoesNotCauseKeyCollisions()
        {
            var words = Words("catnip", "cats", "cat", "catnap"); // "cat", being the common prefix
            var trie = new Trie<Char, String> { Lookup(words) };

            foreach(var word in words) {
                Assert.True(trie.TryGetValue(word, out var value));
                Assert.Equal(word, value);
            }
        }

        [Fact]
        public void TheLastKeyAddedToTrieWins()
        {
            var discardedInTrie = (key: "duplicate", value: 5);
            var lookup = new List<(String key, Int32 value)>
            {
                ("first" , 3),
                discardedInTrie,
                (discardedInTrie.key , discardedInTrie.value+2),
                ("last" , 1)
            };
            var trie = new Trie<Char, Int32> { lookup };

            var actual = lookup.Select(l => (l.key, trie.TryGetValue(l.key, out var r) ? r : -1));
            lookup.Remove(discardedInTrie);
            Assert.Equal(lookup, actual);
        }

        private static String[] Words(params String[] words) => words;
        private static IEnumerable<(String, String)> Lookup(params String[] words) => words.Select(w => (w, w));
    }
}
