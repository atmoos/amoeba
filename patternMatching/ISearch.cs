using System;
using System.Collections.Generic;

namespace patternMatching
{
    public interface ISearch<TAlphabet, TEval>
        where TEval : IEnumerable<TAlphabet>
    {
        IEnumerable<TEval> Search(TEval input);
    }
}
