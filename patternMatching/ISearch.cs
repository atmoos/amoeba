using System;
using System.Collections.Generic;

namespace patternMatching
{
    public interface ISearch<TAlphabet, TSearchOutput>
    {
        IEnumerable<TSearchOutput> Search(IEnumerable<TAlphabet> input);
    }
}
