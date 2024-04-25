namespace Chatapp.Core.Results
{
    public interface IError<out T>
    {
        T ErrorValue { get; }
    }
}
