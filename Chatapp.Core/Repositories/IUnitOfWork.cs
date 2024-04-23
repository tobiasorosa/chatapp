using Chatapp.Core.Results;

namespace Chatapp.Core.Repositories
{
    public interface IUnitOfWork
    {
        ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<int> SaveChangesAsync(bool publishEvents, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> ExecuteInTrasactionAsync(Func<Task> action, bool publishEvents = false);
        Task<Result> ExecuteInTrasactionAsync(Func<Task<Result>> action, bool publishEvents = false);
        Task<Result<T>> ExecuteInTrasactionAsync<T>(Func<Task<Result<T>>> action, bool publishEvents = false);
        Task<Result<T, E>> ExecuteInTrasactionAsync<T, E>(Func<Task<Result<T, E>>> action, bool publishEvents = false);
        ValueTask RollbackAsync();
    }
}
