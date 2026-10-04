using MediatR;

namespace Application.Reporters.Commands.CreateReporter;

public sealed record CreateReporterCommand(Guid UserId) : IRequest<CreateReporterResult?>;

public sealed record CreateReporterResult(ReporterDto Reporter, bool Created);
