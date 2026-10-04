using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentScope;

public sealed record ChangeIncidentScopeCommand(
    Guid IncidentId,
    Guid UserId,
    bool IsAdmin,
    string Name) : IRequest<IncidentDto?>;
