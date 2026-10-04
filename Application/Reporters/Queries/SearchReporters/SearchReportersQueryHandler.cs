using Application.Abstractions.Reporters;
using Application.Common;
using Core.Reporters.ReportStates;
using MediatR;

namespace Application.Reporters.Queries.SearchReporters;

public sealed class SearchReportersQueryHandler(IReporterRepository reporterRepository)
    : IRequestHandler<SearchReportersQuery, PagedResult<ReporterDto>>
{
    public async Task<PagedResult<ReporterDto>> Handle(
        SearchReportersQuery request,
        CancellationToken cancellationToken)
    {
        var state = ReporterStateFor(request.State);
        var result = await reporterRepository.SearchAsync(
            state,
            request.Page,
            request.PageSize,
            cancellationToken);

        return PagedResult<ReporterDto>.Create(
            result.Items.Select(ReporterDto.From).ToList(),
            result.Page,
            result.PageSize,
            result.TotalCount);
    }

    private static ReporterState? ReporterStateFor(string? state) => state switch
    {
        null or "" => null,
        "Active" => new ActiveReporterState(),
        "Suspended" => new SuspendedReporterState(),
        "Blocked" => new BlockedReporterState(),
        _ => throw new InvalidOperationException("Reporter state must be validated before handling the query.")
    };
}
