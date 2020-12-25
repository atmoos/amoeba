using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace patternMatching.Tests
{
    public abstract class TestSearchAlgorithm<TSearch>
        where TSearch : ISearchBuilder<Char, String>, new()
    {
        [Fact]
        public void NoMatchReturnsAnEmptyEnumerable()
        {
            var trie = SearchFor("One", "two");

            var match = trie.Search("seven eight ten");

            Assert.Equal(Enumerable.Empty<String>(), match);
        }

        [Fact]
        public void DuplicatePatternEntriesDoesNotCauseDuplicateMatches()
        {
            var trie = SearchFor("one", "two", "two", "three");

            var match = trie.Search("one two three");

            Assert.Equal(Result("one", "two", "three"), match);
        }

        [Fact]
        public void SearchMatchesOneOccurrenceAtTheBeginning()
        {
            var trie = SearchFor("ab");

            var match = trie.Search("abca");

            Assert.Equal(Result("ab"), match);
        }

        [Fact]
        public void SearchMatchesOneOccurrenceInTheCenter()
        {
            var trie = SearchFor("ab");

            var match = trie.Search("The ablative");

            Assert.Equal(Result("ab"), match);
        }

        [Fact]
        public void SearchMatchesOneOccurrenceAtTheEnd()
        {
            var trie = SearchFor("act");

            var match = trie.Search("The abstract");

            Assert.Equal(Result("act"), match);
        }

        [Fact]
        public void SearchMatchesEachOccurrenceOnce()
        {
            var trie = SearchFor("Fliegen");

            var match = trie.Search("Wenn Fliegen hinter Fliegen fliegen, fliegen Fliegen Fliegen nach!");

            Assert.Equal(Result("Fliegen", "Fliegen", "Fliegen", "Fliegen"), match);
        }
        [Fact]
        public void SearchMatchesTwoSeparatedPatterns()
        {
            var trie = SearchFor("lee", "luv");

            var match = trie.Search("von luv ins lee");

            Assert.Equal(Result("luv", "lee"), match);
        }

        [Fact]
        public void SearchMatchesAdjacentSuffixPatterns()
        {
            var trie = SearchFor("lee", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("lee", "leeward"), match);
        }

        [Fact]
        public void SearchMatchesAdjacentPostfixPatterns()
        {
            var trie = SearchFor("ward", "leeward");

            var match = trie.Search("turn leeward!");

            Assert.Equal(Result("leeward", "ward"), match);
        }

        [Fact]
        public void SearchMatchesOverlappingSuffixPatterns()
        {
            var trie = SearchFor("ton", "pontons");

            var match = trie.Search("The pontons!");

            Assert.Equal(Result("ton", "pontons"), match);
        }

        [Fact]
        public void SearchMatchesAreReturnedInTheOrderTheyAppearInTheSearchText()
        {
            var trie = SearchFor("seven", "five", "eight", "six");

            var match = trie.Search("... five, six, seven and eight!");

            Assert.Equal(Result("five", "six", "seven", "eight"), match);
        }

        [Fact]
        public void InterleavedPatternsAreResolved()
        {
            var trie = SearchFor("in", "tin", "sting");

            var match = trie.Search("a tin in a stinger");

            Assert.Equal(Result("tin", "in", "in", "tin", "in", "sting"), match);
        }

        [Fact]
        public void IndexesInSourceStringAreFound()
        {
            const String sourceString = "a tin in a stinger";
            var trie = SearchFor("in", "tin", "sting");

            var match = trie.SearchForStartIndices(sourceString, m => (UInt64)m.Length);

            AssertMatchesAreIn(sourceString, match);
        }



        [Fact]
        public void SuffixIsSetToRootWhenNoProperSuffixIsFound()
        {
            // This is a minimal test!
            // I.e: All characters in the assert and act strings are relevant.
            var trie = SearchFor("18", "3"); // there is no proper suffix for '8', hence next state must be root, in order to get to '3'

            var match = trie.Search("183");

            Assert.Equal(Result("18", "3"), match);
        }
        [Fact]
        public void TheImmediateChildrenOfRootMustBeAssignedRootAsSuffix()
        {
            // This is a minimal test!
            // I.e: All characters in the assert and act strings are relevant.
            var trie = SearchFor("16", "36");

            var match = trie.Search("136");

            Assert.Equal(Result("36"), match);
        }

        protected static void AssertMatchesAreIn(String sourceString, IEnumerable<(UInt64 position, String match)> matches)
        {
            foreach(var (position, segment) in matches) {
                Assert.Equal(segment, sourceString.Substring((Int32)position, segment.Length));
            }
        }

        protected static ISearch<Char, String> SearchFor(params String[] patterns) => new TSearch() { patterns }.Build();

        protected static IEnumerable<String> Result(params String[] result) => result;
    }
}
