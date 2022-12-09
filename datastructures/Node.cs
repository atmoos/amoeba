using System;
using System.Collections;
using System.Collections.Generic;

namespace datastructures;

public abstract class Node : IEnumerable<Node>
{
    public abstract Int32 Count { get; }
    public Boolean EndOfWord { get; init; } = false;

    public override String ToString() => ToString(this.EndOfWord ? "*" : String.Empty);
    public abstract String ToString(String wordMark);

    public abstract IEnumerator<Node> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
