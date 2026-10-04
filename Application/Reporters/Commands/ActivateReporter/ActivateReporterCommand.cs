using MediatR;

namespace Application.Reporters.Commands.ActivateReporter;

public sealed record ActivateReporterCommand(Guid ReporterId) : IRequest<bool>;
