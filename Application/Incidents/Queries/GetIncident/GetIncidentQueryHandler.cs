using Application.Abstractions.Incidents;
using Application.Abstractions.Reporters;
using Core.Incidents;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Incidents.Queries.GetIncident;

public sealed class GetIncidentQueryHandler(
    IIncidentRepository incidentRepository,
    IReporterRepository reporterRepository) : IRequestHandler<GetIncidentQuery, IncidentDto?>
{
    public async Task<IncidentDto?> Handle(
        GetIncidentQuery request,
        CancellationToken cancellationToken)
    {
        var incident = await incidentRepository.GetByIdAsync(
            new IncidentId(request.IncidentId),
            cancellationToken);
        if (incident is null)
            return null;
        if (request.IsAdmin)
            return IncidentDto.From(incident);

        var reporter = await reporterRepository.GetByUserIdAsync(
            new UserId(request.UserId),
            cancellationToken);
        return reporter is not null && reporter.Id == incident.ReporterId
            ? IncidentDto.From(incident)
            : null;
    }
}
