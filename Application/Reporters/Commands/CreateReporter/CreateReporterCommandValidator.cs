using FluentValidation;

namespace Application.Reporters.Commands.CreateReporter;

public sealed class CreateReporterCommandValidator : AbstractValidator<CreateReporterCommand>
{
    public CreateReporterCommandValidator() =>
        RuleFor(command => command.UserId).NotEmpty();
}
