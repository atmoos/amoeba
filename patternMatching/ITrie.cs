using System;
using System.Collections.Generic;

namespace patternMatching
{
    public interface ITrie<TAlphabet, TEval>
        where TEval : IEnumerable<TAlphabet>
    {
        IEnumerable<TEval> Search(TEval input);
    }
}
