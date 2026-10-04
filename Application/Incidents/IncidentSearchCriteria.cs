using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Severity;
using Core.Reporters;

namespace Application.Incidents;

public sealed record IncidentSearchCriteria(
    ReporterId? ReporterId,
    IncidentCategoryId? CategoryId,
    IncidentSeverity? Severity,
    InfrastructureEnvironment? Environment,
    bool? IsOverdue,
    int Page,
    int PageSize);
