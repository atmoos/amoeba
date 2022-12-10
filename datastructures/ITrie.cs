using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Data.Structures;

public interface ITrie<TCharacter> : IEnumerable<TCharacter[]>
{
    Int32 Size { get; }
    Int32 Count { get; }
    Node<TCharacter> Root { get; }
    Boolean Contains(IEnumerable<TCharacter> key);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
