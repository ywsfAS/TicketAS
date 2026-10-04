using MediatR;

namespace Application.Reporters.Queries.GetMyReporter;

public sealed record GetMyReporterQuery(Guid UserId) : IRequest<ReporterDto?>;
