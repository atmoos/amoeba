using System;
using Xunit;

namespace patternMatching.Tests;

public sealed class MultiPatternSearchTests : ITestSearchAlgorithm
{
    private readonly ITestSearchAlgorithm tester = new SearchAlgorithmTester<Naive.MultiPatternSearch<Char, String>>();

    [Fact]
    public void DuplicatePatternEntriesDoesNotCauseDuplicateMatches()
    {
        this.tester.DuplicatePatternEntriesDoesNotCauseDuplicateMatches();
    }

    [Fact]
    public void IndexesInSourceStringAreFound()
    {
        this.tester.IndexesInSourceStringAreFound();
    }

    [Fact]
    public void InterleavedPatternsAreResolved()
    {
        this.tester.InterleavedPatternsAreResolved();
    }

    [Fact]
    public void NoMatchReturnsAnEmptyEnumerable()
    {
        this.tester.NoMatchReturnsAnEmptyEnumerable();
    }

    [Fact]
    public void SearchMatchesAdjacentPostfixPatterns()
    {
        this.tester.SearchMatchesAdjacentPostfixPatterns();
    }

    [Fact]
    public void SearchMatchesAdjacentSuffixPatterns()
    {
        this.tester.SearchMatchesAdjacentSuffixPatterns();
    }

    [Fact]
    public void SearchMatchesAreReturnedInTheOrderTheyAppearInTheSearchText()
    {
        this.tester.SearchMatchesAreReturnedInTheOrderTheyAppearInTheSearchText();
    }

    [Fact]
    public void SearchMatchesEachOccurrenceOnce()
    {
        this.tester.SearchMatchesEachOccurrenceOnce();
    }

    [Fact]
    public void SearchMatchesOneOccurrenceAtTheBeginning()
    {
        this.tester.SearchMatchesOneOccurrenceAtTheBeginning();
    }

    [Fact]
    public void SearchMatchesOneOccurrenceAtTheEnd()
    {
        this.tester.SearchMatchesOneOccurrenceAtTheEnd();
    }

    [Fact]
    public void SearchMatchesOneOccurrenceInTheCenter()
    {
        this.tester.SearchMatchesOneOccurrenceInTheCenter();
    }

    [Fact]
    public void SearchMatchesOverlappingSuffixPatterns()
    {
        this.tester.SearchMatchesOverlappingSuffixPatterns();
    }

    [Fact]
    public void SearchMatchesTwoSeparatedPatterns()
    {
        this.tester.SearchMatchesTwoSeparatedPatterns();
    }

    [Fact]
    public void SuffixIsSetToRootWhenNoProperSuffixIsFound()
    {
        this.tester.SuffixIsSetToRootWhenNoProperSuffixIsFound();
    }

    [Fact]
    public void TheImmediateChildrenOfRootMustBeAssignedRootAsSuffix()
    {
        this.tester.TheImmediateChildrenOfRootMustBeAssignedRootAsSuffix();
    }
}
