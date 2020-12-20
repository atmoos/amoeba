using System;
using System.Collections;
using System.Collections.Generic;

namespace patternMatching
{
    public interface ISearchBuilder<TAlphabet, TOnMatch> : IEnumerable
    {
        void Add(IEnumerable<TAlphabet> pattern, in TOnMatch match);
        ISearch<TAlphabet, TOnMatch> Build();

        IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException("Never use a builder in a foreach loop or similar.");
    }
}
