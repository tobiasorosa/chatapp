namespace Chatapp.Core.Results
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}
