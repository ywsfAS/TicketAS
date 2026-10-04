using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Reporters;
using MediatR;

namespace Application.Reporters.Commands.SuspendReporter;

public sealed class SuspendReporterCommandHandler(
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SuspendReporterCommand, bool>
{
    public async Task<bool> Handle(
        SuspendReporterCommand request,
        CancellationToken cancellationToken)
    {
        var reporter = await reporterRepository.GetByIdAsync(
            new ReporterId(request.ReporterId),
            cancellationToken);
        if (reporter is null)
            return false;

        reporter.Suspend();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
