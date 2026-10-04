using MediatR;

namespace Application.Reporters.Commands.BlockReporter;

public sealed record BlockReporterCommand(Guid ReporterId) : IRequest<bool>;
