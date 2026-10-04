using Application.Abstractions.Incidents;
using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Incidents;
using Core.Reporters;
using Core.Users;
using MediatR;

namespace Application.Incidents.Commands.ChangeIncidentTitle;

public sealed class ChangeIncidentTitleCommandHandler(
    IIncidentRepository incidentRepository,
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeIncidentTitleCommand, IncidentDto?>
{
    public async Task<IncidentDto?> Handle(
        ChangeIncidentTitleCommand request,
        CancellationToken cancellationToken)
    {
        var incident = await incidentRepository.GetByIdAsync(
            new IncidentId(request.IncidentId),
            cancellationToken);
        if (incident is null || !await CanChangeAsync(incident, request, cancellationToken))
            return null;

        incident.ChangeTitle(IncidentTitle.Create(request.Text));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return IncidentDto.From(incident);
    }

    private async Task<bool> CanChangeAsync(
        Incident incident,
        ChangeIncidentTitleCommand request,
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
