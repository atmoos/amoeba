namespace Data.Structures;

public interface IState<TLabel>
{
    Node<TLabel>? Next(in TLabel label);
}
