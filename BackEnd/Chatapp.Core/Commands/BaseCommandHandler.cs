using Chatapp.Core.Entities;
using Chatapp.Core.Events;
using Chatapp.Core.Notifications;
using Chatapp.Core.Repositories;
using Chatapp.Core.Results;
using MediatR;

namespace Chatapp.Core.Commands
{
    public class BaseCommandHandler
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMediator _mediator;
        protected readonly NotificationHandler _notifications;

        public BaseCommandHandler(IUnitOfWork uow, IMediator mediator, INotificationHandler<Notification> notifications)
        {
            _unitOfWork = uow;
            _mediator = mediator;
            _notifications = (NotificationHandler)notifications;
        }

        public async Task<bool> CommitV2()
        {
            if (_notifications.HasNotifications()) return false;

            await _unitOfWork.SaveChangesAsync(true);

            return true;
        }

        public async ValueTask RollbackAsync()
        {
            await _unitOfWork.RollbackAsync();
        }

        public async Task<bool> ExecuteInTrasactionV2Async(Func<Task> action)
        {
            return await _unitOfWork.ExecuteInTrasactionAsync(action, true);
        }

        public async Task<Result> ExecuteInTrasactionV2Async(Func<Task<Result>> action)
        {
            return await _unitOfWork.ExecuteInTrasactionAsync(action, true);
        }

        public Unit Error(string value)
        {
            _mediator.Publish(new Notification("erro", value));
            return Unit.Value;
        }

        public T Error<T>(string value)
        {
            _mediator.Publish(new Notification("erro", value));
            return default;
        }

        public async ValueTask PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : Event
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        public async ValueTask PublishAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : Event
        {
            foreach (var @event in events)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }

        public async ValueTask PublishAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            await PublishAsync(entity.Events, cancellationToken);
        }

        public async ValueTask PublishAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default)
        {
            await PublishAsync(entities.SelectMany(e => e.Events), cancellationToken);
        }

        public async ValueTask PublishAsync(params Entity[] entities)
        {
            await PublishAsync(entities.ToList());
        }

        public static Unit Success() => Unit.Value;

        public static T Success<T>(object value) => (T)value;
        public static T Success<T>() => default;

        public bool HasNotifications() => _notifications.HasNotifications();
        public IEnumerable<string> GetNotifications() => _notifications.GetNotifications().Select(n => n.Value);
    }
}
