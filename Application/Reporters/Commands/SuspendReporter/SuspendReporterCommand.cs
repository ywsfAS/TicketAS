using MediatR;

namespace Application.Reporters.Commands.SuspendReporter;

public sealed record SuspendReporterCommand(Guid ReporterId) : IRequest<bool>;
