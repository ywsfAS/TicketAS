using Application.Abstractions.Incidents;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentScope;

public sealed class ChangeIncidentScopeCommandHandler(
    IIncidentRepository incidentRepository,
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeIncidentScopeCommand, IncidentDto?>
{
    public async Task<IncidentDto?> Handle(
        ChangeIncidentScopeCommand request,
        CancellationToken cancellationToken)
    {
        var incident = await incidentRepository.GetByIdAsync(
            new IncidentId(request.IncidentId),
            cancellationToken);
        if (incident is null || !await CanChangeAsync(incident, request, cancellationToken))
            return null;

        var behavior = await incidentRepository.GetCategoryBehaviorByIdAsync(
            incident.CategoryId,
            cancellationToken)
            ?? throw new InvalidOperationException($"Category behavior for category '{incident.CategoryId.Id}' is missing.");
        incident.ChangeScope(IncidentDomainLookups.Scope(request.Name)!, behavior);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return IncidentDto.From(incident);
    }

    private async Task<bool> CanChangeAsync(
        Incident incident,
        ChangeIncidentScopeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.IsAdmin)
            return true;

        var reporter = await reporterRepository.GetByUserIdAsync(
            new UserId(request.UserId),
            cancellationToken);
        return reporter is not null && incident.ReporterId == reporter.Id;
    }

}
