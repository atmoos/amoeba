using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace datastructures;

public interface ITrie<TCharacter> : IEnumerable<TCharacter[]>
{
    Int32 Size { get; }
    Int32 Count { get; }
    Boolean Contains(IEnumerable<TCharacter> key);

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
public interface ITrie<TCharacter, TValue> : ITrie<TCharacter>, IEnumerable<(TCharacter[] key, TValue value)>
{
    TValue this[IEnumerable<TCharacter> key] { get; }
    Boolean TryGetValue(IEnumerable<TCharacter> key, [NotNullWhen(true)] out TValue? value);

    IEnumerator<TCharacter[]> IEnumerable<TCharacter[]>.GetEnumerator()
    {
        foreach (var (key, _) in (IEnumerable<(TCharacter[] key, TValue value)>)this) {
            yield return key;
        }
    }
}
