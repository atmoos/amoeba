using System.Collections.Generic;

namespace patternMatching;

public interface ISearch<TAlphabet, TSearchOutput>
{
    IEnumerable<TSearchOutput> Search<TText>(TText input) where TText : IEnumerable<TAlphabet>;
}
