namespace Core.Utilities
{
    // Base type for strongly typed identifiers.
    // Each identifier wraps a Guid while remaining a distinct domain type.
    public abstract record StrongTypedId(Guid Value)
    {
        public static explicit operator Guid(StrongTypedId id) => id.Value;
    };
}
