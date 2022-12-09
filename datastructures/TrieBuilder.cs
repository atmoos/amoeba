using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace datastructures;

public sealed class TrieBuilder<TCharacter> : ITrieBuilder<TCharacter>, ITrie<TCharacter>
    where TCharacter : notnull
{
    private Int32 size;
    private readonly RecursiveNode root = RecursiveNode.Root();
    public Node Root => this.root;
    internal RecursiveNode RecursionRoot => this.root;
    public Int32 Size => this.size;
    public Int32 Count { get; private set; }
    public void Add(IEnumerable<TCharacter> value) => AddKey(value);
    public Boolean Contains(IEnumerable<TCharacter> key)
    {
        var child = this.root;
        foreach (var label in key) {
            if ((child = child.Next(in label)) == null) {
                return false;
            }
        }
        return true;
    }
    private RecursiveNode AddKey(IEnumerable<TCharacter> key)
    {
        (RecursiveNode node, TCharacter label)? shift = null;
        foreach (var label in key) {
            if (shift is (RecursiveNode, TCharacter) prev) {
                var next = prev.node.Add(prev.label, ref this.size);
                shift = (next, label);
                continue;
            }
            shift = (this.root, label);
        }
        if (shift is (RecursiveNode, TCharacter) end) {
            var prevSize = this.size;
            var tailMark = end.node.AddEnd(end.label, ref this.size);
            if (this.size > prevSize) {
                this.Count++;
            }
            return tailMark;
        }
        return this.root;
    }

    public ITrie<TCharacter> Build()
    {
        return Trie<TCharacter>.Compress(this);
    }

    public IEnumerator<TCharacter[]> GetEnumerator() => this.root.DepthFirst(new List<TCharacter>()).GetEnumerator();

    internal sealed class RecursiveNode : Node
    {
        private readonly Dictionary<TCharacter, RecursiveNode> children;
        public override Int32 Count => this.children.Count;
        public IEnumerable<KeyValuePair<TCharacter, RecursiveNode>> Children => this.children;
        private RecursiveNode() => this.children = new Dictionary<TCharacter, RecursiveNode>();
        private RecursiveNode(RecursiveNode node) => this.children = node.children;
        internal RecursiveNode Add(in TCharacter label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                return child;
            }
            counter++;
            return this.children[label] = new RecursiveNode { EndOfWord = false };
        }
        internal RecursiveNode AddEnd(in TCharacter label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                if (child.EndOfWord) {
                    return child;
                }
                return this.children[label] = new RecursiveNode(child) { EndOfWord = true };
            }
            counter++;
            return this.children[label] = new RecursiveNode { EndOfWord = true };
        }
        public RecursiveNode? Next(in TCharacter label) => this.children.TryGetValue(label, out var child) ? child : null;
        public IEnumerable<TCharacter[]> DepthFirst(List<TCharacter> prefix)
        {
            if (this.EndOfWord) {
                yield return prefix.ToArray();
            }
            foreach (var (label, child) in this.children) {
                foreach (var word in child.DepthFirst(new List<TCharacter>(prefix) { label })) {
                    yield return word;
                }
            }
        }
        public (TCharacter[] prefix, RecursiveNode leaf) Compress()
        {
            if (this.EndOfWord || this.children.Count > 1) {
                return (Array.Empty<TCharacter>(), this);
            }
            var children = this.children;
            var (label, onlyChild) = children.Single();
            var prefix = new List<TCharacter>() { label };
            while (!onlyChild.EndOfWord && (children = onlyChild.children).Count == 1) {
                (label, onlyChild) = children.Single();
                prefix.Add(label);
            }
            return (prefix.ToArray(), onlyChild);
        }

        public override String ToString(String wordMark)
        {
            String children = String.Join(";", this.children.Keys);
            return $"[{children}]{wordMark}";
        }

        public override IEnumerator<Node> GetEnumerator() => this.children.Values.GetEnumerator();
        internal static RecursiveNode Root() => new();
    }
}
