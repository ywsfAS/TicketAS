using FluentValidation;

namespace Application.Reporters.Commands.BlockReporter;

public sealed class BlockReporterCommandValidator : AbstractValidator<BlockReporterCommand>
{
    public BlockReporterCommandValidator() =>
        RuleFor(command => command.ReporterId).NotEmpty();
}
