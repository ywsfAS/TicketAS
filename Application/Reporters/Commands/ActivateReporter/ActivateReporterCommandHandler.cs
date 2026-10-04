using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Reporters;
using MediatR;

namespace Application.Reporters.Commands.ActivateReporter;

public sealed class ActivateReporterCommandHandler(
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivateReporterCommand, bool>
{
    public async Task<bool> Handle(
        ActivateReporterCommand request,
        CancellationToken cancellationToken)
    {
        var reporter = await reporterRepository.GetByIdAsync(
            new ReporterId(request.ReporterId),
            cancellationToken);
        if (reporter is null)
            return false;

        reporter.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
