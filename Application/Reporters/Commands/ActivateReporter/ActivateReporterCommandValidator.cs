using FluentValidation;

namespace Application.Reporters.Commands.ActivateReporter;

public sealed class ActivateReporterCommandValidator : AbstractValidator<ActivateReporterCommand>
{
    public ActivateReporterCommandValidator() =>
        RuleFor(command => command.ReporterId).NotEmpty();
}
