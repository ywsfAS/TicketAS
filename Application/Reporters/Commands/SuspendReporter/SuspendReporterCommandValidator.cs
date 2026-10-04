using FluentValidation;

namespace Application.Reporters.Commands.SuspendReporter;

public sealed class SuspendReporterCommandValidator : AbstractValidator<SuspendReporterCommand>
{
    public SuspendReporterCommandValidator() =>
        RuleFor(command => command.ReporterId).NotEmpty();
}
