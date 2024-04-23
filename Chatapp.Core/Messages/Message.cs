namespace Chatapp.Core.Messages
{
    public abstract class Message
    {
        public Guid Id { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        protected Message()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }
}
