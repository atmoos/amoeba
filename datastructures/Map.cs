using System;
using System.Collections.Generic;

namespace datastructures
{
    public sealed class Map<TKey, TValue> where TValue : class
    {
        private readonly Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();
        public Map(Int32 size) => this.map = new Dictionary<TKey, TValue>(size);
        public TValue this[in TKey key] {
            get => this.map.TryGetValue(key, out var value) ? value : null;
            set => this.map[key] = value;
        }
    }
}