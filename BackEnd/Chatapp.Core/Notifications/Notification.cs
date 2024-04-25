using MediatR;

namespace Chatapp.Core.Notifications
{
    public class Notification : INotification
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        public Notification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
