using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentTitle;

public sealed record ChangeIncidentTitleCommand(
    Guid IncidentId,
    Guid UserId,
    bool IsAdmin,
    string Text) : IRequest<IncidentDto?>;
