using System;
using System.Collections.Generic;

namespace datastructures;

public sealed class Map<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue?> map = new();
    public Map(Int32 size) => this.map = new Dictionary<TKey, TValue?>(size);
    public TValue? this[in TKey key]
    {
        get => this.map.TryGetValue(key, out var value) ? value : default;
        set => this.map[key] = value;
    }
}
