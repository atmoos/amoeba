using System;
using System.Linq;
using Xunit;

namespace datastructures.Tests;

public interface ITrieTests
{
    void AllDistinctWordsAreContainedWithinTrie();
    void TrieContainsAllWordPrefixes();
    void TrieContainsAllWords();
    void TrieCountIsEqualToDistinctNumberOfWords();
    void TrieDoesNotContainAnySuffixes();
    void TrieSizeIsCorrectValue();
    void TrieSizeIsSmallerThanNumberOfAddedCharacters();
}

public class TrieTester : ITrieTests
{
    readonly Func<String[], ITrie<Char>> create;
    public TrieTester(Func<String[], ITrie<Char>> create) => this.create = create;
    public void TrieContainsAllWords()
    {
        var words = Words("hello", "ell", "world", "old", "lord");
        var trie = Trie(words);

        Assert.Equal(words.Select(w => (w, true)), words.Select(w => (w, trie.Contains(w))));
    }
    public void TrieContainsAllWordPrefixes()
    {
        var trie = Trie("Gattaca");
        var prefixes = Words("G", "Gat", "Gatt", "Gatta", "Gattac", "Gattaca");

        Assert.Equal(prefixes.Select(p => (p, true)), prefixes.Select(p => (p, trie.Contains(p))));
    }
    public void TrieDoesNotContainAnySuffixes()
    {
        var trie = Trie("Gattaca");
        Assert.False(trie.Contains("a"));
    }
    public void TrieSizeIsCorrectValue()
    {
        var trie = Trie(Words("pool", "prize", "preview", "prepare", "produce", "progress"));

        Assert.Equal(27, trie.Size);
    }
    public void TrieSizeIsSmallerThanNumberOfAddedCharacters()
    {
        var words = Words("pool", "prize", "preview", "prepare", "produce", "progress");
        var trie = Trie(words);
        var charCount = words.SelectMany(w => w).Count();

        Assert.InRange(trie.Size, words.Length, charCount);
    }
    public void TrieCountIsEqualToDistinctNumberOfWords()
    {
        const String duplicate = "mini me";
        var words = Words("3", duplicate, "3232", duplicate, "text");
        var trie = Trie(words);

        Assert.Equal(words.Distinct().Count(), trie.Count);
    }
    public void AllDistinctWordsAreContainedWithinTrie()
    {
        var words = Words("peppermint", "Snoopy", "snoopy", "duplicate", "no", "duplicate", "yes");
        var trie = Trie(words);

        Assert.Equal(words.Distinct(), trie.Select(w => new String(w)));
    }

    private ITrie<Char> Trie(params String[] words) => this.create(words);
    private static String[] Words(params String[] words) => words;
}