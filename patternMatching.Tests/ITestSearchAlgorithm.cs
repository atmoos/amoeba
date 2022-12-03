namespace patternMatching.Tests;

public interface ITestSearchAlgorithm
{
    void DuplicatePatternEntriesDoesNotCauseDuplicateMatches();
    void IndexesInSourceStringAreFound();
    void InterleavedPatternsAreResolved();
    void NoMatchReturnsAnEmptyEnumerable();
    void SearchMatchesAdjacentPostfixPatterns();
    void SearchMatchesAdjacentSuffixPatterns();
    void SearchMatchesAreReturnedInTheOrderTheyAppearInTheSearchText();
    void SearchMatchesEachOccurrenceOnce();
    void SearchMatchesOneOccurrenceAtTheBeginning();
    void SearchMatchesOneOccurrenceAtTheEnd();
    void SearchMatchesOneOccurrenceInTheCenter();
    void SearchMatchesOverlappingSuffixPatterns();
    void SearchMatchesTwoSeparatedPatterns();
    void SuffixIsSetToRootWhenNoProperSuffixIsFound();
    void TheImmediateChildrenOfRootMustBeAssignedRootAsSuffix();
}
