using Application.Abstractions.Reporters;
using Core.Users;
using MediatR;

namespace Application.Reporters.Queries.GetMyReporter;

public sealed class GetMyReporterQueryHandler(IReporterRepository reporterRepository)
    : IRequestHandler<GetMyReporterQuery, ReporterDto?>
{
    public async Task<ReporterDto?> Handle(
        GetMyReporterQuery request,
        CancellationToken cancellationToken)
    {
        var reporter = await reporterRepository.GetByUserIdAsync(
            new UserId(request.UserId),
            cancellationToken);
        return reporter is null ? null : ReporterDto.From(reporter);
    }
}
