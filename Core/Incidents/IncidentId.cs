using Core.Utilities;

namespace Core.Incidents
{
    public sealed record IncidentId(Guid Id) : StrongTypedId(Id);
}
