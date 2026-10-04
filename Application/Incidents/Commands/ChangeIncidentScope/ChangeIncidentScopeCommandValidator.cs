using FluentValidation;

namespace Application.Incidents.Commands.ChangeIncidentScope;

public sealed class ChangeIncidentScopeCommandValidator : AbstractValidator<ChangeIncidentScopeCommand>
{
    public ChangeIncidentScopeCommandValidator()
    {
        RuleFor(command => command.IncidentId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Name)
            .Must(name => IncidentDomainLookups.Scope(name) is not null)
            .WithMessage("Scope must be SingleUser, Department, or Organization.");
    }
}
