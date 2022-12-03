using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace patternMatching.Tests;

public sealed class AhoCorasickTests : ITestSearchAlgorithm
{
    private readonly ITestSearchAlgorithm tester = new SearchAlgorithmTester<AhoCorasick<Char, String>>();

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

    [Fact]
    public void AhoCorasickIsFasterThanNaiveApproach()
    {
        var warmup = TimeMatching(20, 800);
        var run = TimeMatching(40, 8000);
        Assert.True(run.naive > run.aho);
        Assert.NotEqual(warmup, run);
    }

    private static (TimeSpan naive, TimeSpan aho) TimeMatching(Int32 dictSize, Int32 textSize)
    {
        var rand = new Random(dictSize);
        var text = CreateText(rand, textSize);
        var dictionary = Enumerable.Range(0, dictSize).Select(_ => rand.Next(dictSize, textSize).ToString()).ToArray();
        var trie = new AhoCorasick<Char, String> { dictionary }.Build();
        var naive = new Naive.MultiPatternSearch<Char, String> { dictionary }.Build();

        var timer = Stopwatch.StartNew();
        var trieMatches = trie.Search(text).ToList();
        var trieTiming = timer.Elapsed;

        timer.Restart();
        var naiveMatches = naive.Search(text).ToList();
        var naiveTiming = timer.Elapsed;

        Assert.Equal(String.Join("|", naiveMatches), String.Join("|", trieMatches));
        Assert.Equal(naiveMatches, trieMatches);
        return (naiveTiming, trieTiming);
    }

    private static String CreateText(Random rand, Int32 inputSize)
    {
        const Int32 scale = 23;
        var text = new StringBuilder();
        var maxInteger = scale * inputSize;
        for (Int32 i = 0; i < inputSize; i++) {
            var next = rand.Next(10, maxInteger);
            text.Append(next);
            if (next % 7 == 0) {
                text.Append(" ");
                continue;
            }
            if (next % 53 == 0) {
                text.Append(". ");
            }
        }
        return text.ToString();
    }
}
