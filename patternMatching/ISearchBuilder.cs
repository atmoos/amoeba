using System;
using System.Collections;
using System.Collections.Generic;

namespace patternMatching
{
    public interface ISearchBuilder<TAlphabet, TEval> : IEnumerable
        where TEval : IEnumerable<TAlphabet>
    {
        void Add(TEval pattern);
        ISearch<TAlphabet, TEval> Build();

        IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException("Never use a builder in a foreach loop or similar.");
    }
}
