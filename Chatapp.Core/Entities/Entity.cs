using Chatapp.Core.Events;

namespace Chatapp.Core.Entities
{
    public abstract class Entity
    {
        protected List<Event> _events;

        public int Id { get; protected set; }

        public IReadOnlyList<Event> Events => _events?.AsReadOnly() ?? new List<Event>().AsReadOnly();

        protected Entity()
        {
            _events = new List<Event>();
        }

        public override bool Equals(object obj)
        {
            Entity entity = obj as Entity;
            if ((object)this == entity)
            {
                return true;
            }

            if ((object)entity == null)
            {
                return false;
            }

            return Id.Equals(entity.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }

            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }

        public bool ExisteId()
        {
            return Id > 0;
        }

        public virtual void IncluirId(int id)
        {
            Id = id;
        }

        public void ClearDomainEvents()
        {
            _events?.Clear();
        }
    }
}
