using MediatR;

namespace Application.Incidents.Queries.GetIncident;

public sealed record GetIncidentQuery(
    Guid IncidentId,
    Guid UserId,
    bool IsAdmin) : IRequest<IncidentDto?>;
