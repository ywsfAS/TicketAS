using Application.Abstractions.Persistence;
using Application.Abstractions.Reporters;
using Core.Reporters;
using MediatR;

namespace Application.Reporters.Commands.BlockReporter;

public sealed class BlockReporterCommandHandler(
    IReporterRepository reporterRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BlockReporterCommand, bool>
{
    public async Task<bool> Handle(
        BlockReporterCommand request,
        CancellationToken cancellationToken)
    {
        var reporter = await reporterRepository.GetByIdAsync(
            new ReporterId(request.ReporterId),
            cancellationToken);
        if (reporter is null)
            return false;

        reporter.Block();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
