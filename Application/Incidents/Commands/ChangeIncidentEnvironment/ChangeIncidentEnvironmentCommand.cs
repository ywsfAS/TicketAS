using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentEnvironment;

public sealed record ChangeIncidentEnvironmentCommand(
    Guid IncidentId,
    Guid UserId,
    bool IsAdmin,
    string Name) : IRequest<IncidentDto?>;
