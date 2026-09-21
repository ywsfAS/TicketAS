namespace Core.Utilities
{
    // Strongly typed identifier for domain events.
    public sealed record EventId(Guid Id): StrongTypedId(Id);

    // Represents an event that occurred within the domain.
    // Domain events describe something that has already happened.
    public interface IDomainEvent
    {
        EventId Id { get; }
        DateTime OccuredAt {  get; }

    }
}
