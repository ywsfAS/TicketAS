using Application.Abstractions.Incidents;
using Application.Abstractions.Reporters;
using Application.Common;
using Core.Incidents.Categories;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Incidents.Queries.SearchIncidents;

public sealed class SearchIncidentsQueryHandler(
    IIncidentRepository incidentRepository,
    IReporterRepository reporterRepository) : IRequestHandler<SearchIncidentsQuery, PagedResult<IncidentDto>>
{
    public async Task<PagedResult<IncidentDto>> Handle(
        SearchIncidentsQuery request,
        CancellationToken cancellationToken)
    {
        ReporterId? reporterId;
        if (request.IsAdmin)
        {
            reporterId = request.ReporterId is Guid selectedReporterId
                ? new ReporterId(selectedReporterId)
                : null;
        }
        else
        {
            var reporter = await reporterRepository.GetByUserIdAsync(
                new UserId(request.UserId),
                cancellationToken);
            if (reporter is null)
                return PagedResult<IncidentDto>.Create([], request.Page, request.PageSize, 0);

            reporterId = reporter.Id;
        }

        var criteria = new IncidentSearchCriteria(
            reporterId,
            request.CategoryId is Guid categoryId ? new IncidentCategoryId(categoryId) : null,
            string.IsNullOrWhiteSpace(request.Severity)
                ? null
                : IncidentDomainLookups.Severity(request.Severity),
            string.IsNullOrWhiteSpace(request.Environment)
                ? null
                : IncidentDomainLookups.Environment(request.Environment),
            request.Overdue,
            request.Page,
            request.PageSize);
        var result = await incidentRepository.SearchAsync(criteria, cancellationToken);

        return PagedResult<IncidentDto>.Create(
            result.Items.Select(IncidentDto.From).ToList(),
            result.Page,
            result.PageSize,
            result.TotalCount);
    }
}
