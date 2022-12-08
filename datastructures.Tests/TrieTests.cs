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
        var trie = new TrieBuilder<Char> { words };

        Assert.Equal(words.Select(w => (w, true)), words.Select(w => (w, trie.Contains(w))));
    }

    [Fact]
    public void TrieContainsAllWordPrefixes()
    {
        var trie = new TrieBuilder<Char> { "Gattaca" };
        var prefixes = Words("G", "Gat", "Gatt", "Gatta", "Gattac", "Gattaca");

        Assert.Equal(prefixes.Select(p => (p, true)), prefixes.Select(p => (p, trie.Contains(p))));
    }

    [Fact]
    public void TrieDoesNotContainAnySuffixes()
    {
        var trie = new TrieBuilder<Char> { "Gattaca" };
        Assert.False(trie.Contains("a"));
    }

    [Fact]
    public void TrieSizeIsCorrectValue()
    {
        var trie = new TrieBuilder<Char> { Words("pool", "prize", "preview", "prepare", "produce", "progress") };

        Assert.Equal(27, trie.Size);
    }

    [Fact]
    public void TrieSizeIsSmallerThanNumberOfAddedCharacters()
    {
        var words = Words("pool", "prize", "preview", "prepare", "produce", "progress");
        var trie = new TrieBuilder<Char> { words };
        var charCount = words.SelectMany(w => w).Count();

        Assert.InRange(trie.Size, words.Length, charCount);
    }
    [Fact]
    public void TrieCountIsEqualToDistinctNumberOfWords()
    {
        const String duplicate = "mini me";
        var words = Words("3", duplicate, "3232", duplicate, "text");
        var trie = new TrieBuilder<Char> { words };

        Assert.Equal(words.Distinct().Count(), trie.Count);
    }

    [Fact]
    public void AllDistinctWordsAreContainedWithinTrie()
    {
        var words = Words("peppermint", "Snoopy", "snoopy", "duplicate", "no", "duplicate", "yes");
        var trie = new TrieBuilder<Char> { words };

        Assert.Equal(words.Distinct(), trie.Select(w => new String(w)));
    }

    private static String[] Words(params String[] words) => words;
}
