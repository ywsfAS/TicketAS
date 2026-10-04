using Application.Common;
using MediatR;

namespace Application.Incidents.Queries.SearchIncidents;

public sealed record SearchIncidentsQuery(
    Guid UserId,
    bool IsAdmin,
    Guid? ReporterId = null,
    Guid? CategoryId = null,
    string? Severity = null,
    string? Environment = null,
    bool? Overdue = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<IncidentDto>>;
