namespace Chatapp.Core.Results
{
    public interface IResult<out T, out E> : IResult, IValue<T>, IError<E>
    {
    }

    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }
}
