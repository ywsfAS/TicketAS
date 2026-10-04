using MediatR;

namespace Application.Incidents.Commands.ReportIncident;

public sealed record ReportIncidentCommand(
    Guid UserId,
    string Problem,
    string Service,
    string Description,
    Guid CategoryId,
    string Scope,
    string Environment) : IRequest<ReportIncidentResult>;

public sealed record ReportIncidentResult(IncidentDto? Incident, bool IsNetworkCategory);
