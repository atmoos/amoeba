using System.Collections.Generic;

namespace datastructures;
internal interface IBuilder<out T>
{
    public T Build();
}

public interface ITrieBuilder<TCharacter>
{
    public void Add(IEnumerable<TCharacter> value);
    ITrie<TCharacter> Build();
}
public interface ITrieBuilder<TCharacter, TValue>
{
    public void Add(IEnumerable<TCharacter> key, TValue value);
    ITrie<TCharacter, TValue> Build();
}
