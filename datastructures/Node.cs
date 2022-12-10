using System.Collections;

namespace Data.Structures;

public abstract class Node<TLabel> : IEnumerable<(TLabel label, Node<TLabel> node)>
{
    public abstract Int32 Count { get; }
    public Boolean EndOfWord { get; init; } = false;
    public abstract IState<TLabel> Walk();

    public abstract IEnumerator<(TLabel label, Node<TLabel> node)> GetEnumerator();

    public override String ToString() => ToString(this.EndOfWord ? "*" : String.Empty);
    public abstract String ToString(String wordMark);

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
