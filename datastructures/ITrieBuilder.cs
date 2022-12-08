using System.Collections.Generic;

namespace datastructures;

public interface ITrieBuilder<TCharacter, TValue>
{
    public void Add(IEnumerable<TCharacter> key, TValue value);
    ITrie<TCharacter, TValue> Build();
}
