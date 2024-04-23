using Chatapp.Core.Messages;
using MediatR;

namespace Chatapp.Core.Events
{
    public class Event : Message, INotification
    {
        protected virtual IEnumerable<Type>? SupersetFor => null;

        public bool IsSupersetFor(Event domainEvent)
        {
            Event domainEvent2 = domainEvent;
            if (domainEvent2 != null && SupersetFor != null)
            {
                return SupersetFor.Any((Type type) => type == domainEvent2.GetType());
            }

            return false;
        }
    }
}
