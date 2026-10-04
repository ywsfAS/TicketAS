using FluentValidation;

namespace Application.Incidents.Commands.ChangeIncidentEnvironment;

public sealed class ChangeIncidentEnvironmentCommandValidator : AbstractValidator<ChangeIncidentEnvironmentCommand>
{
    public ChangeIncidentEnvironmentCommandValidator()
    {
        RuleFor(command => command.IncidentId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Name)
            .Must(name => IncidentDomainLookups.Environment(name) is not null)
            .WithMessage("Environment must be Development, Staging, or Production.");
    }
}
