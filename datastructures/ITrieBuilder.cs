using System.Collections.Generic;

namespace datastructures;

public interface IBuilder<out T>
{
    T Build();
}

public interface ITrieBuilder<TCharacter> : IBuilder<ITrie<TCharacter>>
{
    Node<TCharacter> AddKey(IEnumerable<TCharacter> value);
}
public interface ITrieBuilder<TCharacter, TValue> : IBuilder<ITrie<TCharacter, TValue>>
{
    public void Add(IEnumerable<TCharacter> key, TValue value);
}
