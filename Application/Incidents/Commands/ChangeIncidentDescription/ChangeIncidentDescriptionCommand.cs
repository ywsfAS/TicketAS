using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentDescription;

public sealed record ChangeIncidentDescriptionCommand(
    Guid IncidentId,
    Guid UserId,
    bool IsAdmin,
    string Text) : IRequest<IncidentDto?>;
