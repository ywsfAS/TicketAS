using Application.Common;
using MediatR;

namespace Application.Reporters.Queries.SearchReporters;

public sealed record SearchReportersQuery(
    string? State,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<ReporterDto>>;
