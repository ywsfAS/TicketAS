using Core.Incidents;

namespace Application.Incidents;

public sealed record IncidentDto(
    Guid Id,
    Guid ReporterId,
    Guid CategoryId,
    string Category,
    string Problem,
    string Service,
    string Description,
    string Scope,
    string Environment,
    string Severity,
    DateTime AcknowledgementDueAt,
    DateTime ResolutionDueAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsOverdue)
{
    public static IncidentDto From(Incident incident) =>
        new(
            incident.Id.Id,
            incident.ReporterId.Id,
            incident.CategoryId.Id,
            incident.Category.Name,
            incident.Title.Problem,
            incident.Title.AffectedService,
            incident.Description.Description,
            incident.Scope.Name,
            incident.Environment.Name,
            incident.IncidentSeverity.Name,
            incident.IncidentSla.Acknowledgement.DueAt,
            incident.IncidentSla.Resolution.DueAt,
            incident.CreatedAt,
            incident.UpdatedAt,
            incident.IncidentSla.Resolution.DueAt < DateTime.UtcNow);
}
