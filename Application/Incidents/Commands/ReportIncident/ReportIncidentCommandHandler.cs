using Application.Abstractions.Incidents;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Incidents.Commands.ReportIncident;

public sealed class ReportIncidentCommandHandler(
    IReporterRepository reporterRepository,
    IIncidentRepository incidentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReportIncidentCommand, ReportIncidentResult>
{
    public async Task<ReportIncidentResult> Handle(
        ReportIncidentCommand request,
        CancellationToken cancellationToken)
    {
        var reporter = await reporterRepository.GetByUserIdAsync(
            new UserId(request.UserId),
            cancellationToken);
        if (reporter is null)
            return new ReportIncidentResult(null, true);

        var category = await incidentRepository.GetCategoryByIdAsync(
            new IncidentCategoryId(request.CategoryId),
            cancellationToken);
        if (category is null)
            return new ReportIncidentResult(null, true);
        if (!string.Equals(category.Name, "Network", StringComparison.OrdinalIgnoreCase))
            return new ReportIncidentResult(null, false);

        var behavior = await incidentRepository.GetCategoryBehaviorByIdAsync(
            category.Id,
            cancellationToken);
        if (behavior is null)
            throw new InvalidOperationException($"Network behavior configuration for category '{category.Id.Id}' is missing.");

        var incident = Incident.Create(
            IncidentTitle.Create(request.Problem, request.Service),
            IncidentDescription.Create(request.Description),
            reporter,
            category,
            behavior,
            IncidentDomainLookups.Scope(request.Scope)!,
            IncidentDomainLookups.Environment(request.Environment)!);

        reporter.ReportIncident(incident);
        await incidentRepository.AddAsync(incident, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReportIncidentResult(IncidentDto.From(incident), true);
    }
}
