namespace Core.Utilities
{
    // Base class for all domain entities
    // T respresents the entity's strongly typed identifier.
    public abstract class Entity<T> where T : StrongTypedId 
    {
        // Identity of an entity
        public T Id {  get; protected set; }

        // Domain events raised by this entity.
        private List<IDomainEvent> _events = new List<IDomainEvent>();

        // Exposes domain events as readonly collection
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        public void AddEvent(IDomainEvent @event)
        {
            this._events.Add(@event);
        }
        public void Clear()
        {
            this._events.Clear();
        }
        
    }
}
