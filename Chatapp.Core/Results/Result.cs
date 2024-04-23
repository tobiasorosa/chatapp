namespace Chatapp.Core.Results
{
    public struct Result<T, E> : IResult<T, E>, IResult, IValue<T>, IError<E>
    {
        public bool IsSuccess { get; private set; }

        public string Error { get; private set; }

        public T Value { get; private set; }

        public bool IsFailure => !IsSuccess;

        public E ErrorValue { get; private set; }

        private Result(bool isSuccess, string error, T value, E errorValue)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
            ErrorValue = errorValue;
        }

        public static Result<T, E> Success()
        {
            return new Result<T, E>(isSuccess: true, string.Empty, default(T), default(E));
        }

        public static Result<T, E> Success(T data)
        {
            return new Result<T, E>(isSuccess: true, string.Empty, data, default(E));
        }

        public static Result<T, E> Failure(string error)
        {
            return new Result<T, E>(isSuccess: false, error, default(T), default(E));
        }

        public static Result<T, E> Failure(E error)
        {
            return new Result<T, E>(isSuccess: false, string.Empty, default(T), error);
        }

        public static Result<T, E> Failure(string message, Exception ex)
        {
            return Failure(string.Format("{0}. Exception: {1} Message: {2} Inner Exception: {3} Stack: {4}", message, "Exception", ex.Message, ex.InnerException, ex.StackTrace));
        }

        public static Result<T, E> Failure(string message, E item)
        {
            return new Result<T, E>(isSuccess: false, message, default(T), item);
        }

        public static implicit operator Result(Result<T, E> result)
        {
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Error);
            }

            return Result.Success();
        }

        public static implicit operator Result<T, E>(Result result)
        {
            if (!result.IsSuccess)
            {
                return Failure(result.Error);
            }

            return Success();
        }
    }

    public struct Result : IResult
    {
        public bool IsSuccess { get; private set; }

        public string Error { get; private set; }

        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
        {
            return new Result(isSuccess: true, string.Empty);
        }

        public static Result Failure(string error)
        {
            return new Result(isSuccess: false, error);
        }

        public static Result Failure(string message, Exception ex)
        {
            return Failure(string.Format("{0}. Exception: {1} Message: {2} Inner Exception: {3} Stack: {4}", message, "Exception", ex.Message, ex.InnerException, ex.StackTrace));
        }
    }

    public struct Result<T> : IResult, IValue<T>
    {
        public bool IsSuccess { get; private set; }

        public string Error { get; private set; }

        public T Value { get; private set; }

        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, string error, T value)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public static Result<T> Success()
        {
            return new Result<T>(isSuccess: true, string.Empty, default(T));
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(isSuccess: true, string.Empty, data);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(isSuccess: false, error, default(T));
        }

        public static Result<T> Failure(string message, Exception ex)
        {
            return Failure(string.Format("{0}. Exception: {1} Message: {2} Inner Exception: {3} Stack: {4}", message, "Exception", ex.Message, ex.InnerException, ex.StackTrace));
        }

        public static implicit operator Result(Result<T> result)
        {
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Error);
            }

            return Result.Success();
        }

        public static implicit operator Result<T>(Result result)
        {
            if (!result.IsSuccess)
            {
                return Failure(result.Error);
            }

            return Success();
        }
    }
}
