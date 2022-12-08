using System;
using System.Linq;
using Xunit;

namespace datastructures.Tests;

public class TrieTests
{
    [Fact]
    public void TrieContainsAllWords()
    {
        var words = Words("hello", "ell", "world", "old", "lord");
        var trie = new TrieBuilder<Char, String> { words };

        foreach (var word in words) {
            Assert.True(trie.Contains(word));
        }
    }

    [Fact]
    public void TrieContainsAllPrefixes()
    {
        var trie = new TrieBuilder<Char, String> { "Gattaca" };
        var prefixes = Words("G", "Gat", "Gatt", "Gatta", "Gattac", "Gattaca");

        foreach (var prefix in prefixes) {
            Assert.True(trie.Contains(prefix));
        }
    }

    [Fact]
    public void TrieDoesNotContainAnySuffixes()
    {
        var trie = new TrieBuilder<Char, String> { "Gattaca" };
        Assert.False(trie.Contains("a"));
    }

    [Fact]
    public void TrieSizeIsCorrectValue()
    {
        var trie = new TrieBuilder<Char, String> { Words("pool", "prize", "preview", "prepare", "produce", "progress") };

        Assert.Equal(27, trie.Size);
    }

    [Fact]
    public void TrieSizeIsSmallerThanNumberOfAddedCharacters()
    {
        var words = Words("pool", "prize", "preview", "prepare", "produce", "progress");
        var trie = new TrieBuilder<Char, String> { words };
        var charCount = words.SelectMany(w => w).Count();

        Assert.InRange(trie.Size, words.Length, charCount);
    }


    private static String[] Words(params String[] words) => words;
}
