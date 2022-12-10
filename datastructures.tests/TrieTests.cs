using System;
using Xunit;

namespace Data.Structures.Tests;

public class TrieTests : ITrieTests
{
    private readonly ITrieTests tester = new TrieTester(words => new TrieBuilder<Char> { words }.Build());

    [Fact]
    public void AllDistinctWordsAreContainedWithinTrie() => this.tester.AllDistinctWordsAreContainedWithinTrie();

    [Fact]
    public void TrieContainsAllWordPrefixes() => this.tester.TrieContainsAllWordPrefixes();

    [Fact]
    public void TrieContainsAllWords() => this.tester.TrieContainsAllWords();

    [Fact]
    public void TrieCountIsEqualToDistinctNumberOfWords() => this.tester.TrieCountIsEqualToDistinctNumberOfWords();

    [Fact]
    public void TrieDoesNotContainAnySuffixes() => this.tester.TrieDoesNotContainAnySuffixes();

    [Fact]
    public void TrieSizeIsCorrectValue() => this.tester.TrieSizeIsCorrectValue();

    [Fact]
    public void TrieSizeIsSmallerThanNumberOfAddedCharacters() => this.tester.TrieSizeIsSmallerThanNumberOfAddedCharacters();
}
