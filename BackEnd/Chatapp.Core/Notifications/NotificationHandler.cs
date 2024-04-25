using MediatR;

namespace Chatapp.Core.Notifications
{
    public class NotificationHandler : INotificationHandler<Notification>
    {
        private readonly List<Notification> _notifications;
        public NotificationHandler()
        {
            _notifications = new List<Notification>();
        }

        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public virtual List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }
    }
}
