using Core.Incidents;
using FluentValidation;

namespace Application.Incidents.Commands.ReportIncident;

public sealed class ReportIncidentCommandValidator : AbstractValidator<ReportIncidentCommand>
{
    public ReportIncidentCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Problem)
            .Must(value => !string.IsNullOrWhiteSpace(value)
                && IncidentTitle.IsValidProblem(value.Trim()))
            .WithMessage("Problem must be valid and no longer than 200 characters.");
        RuleFor(command => command.Service)
            .Must(value => !string.IsNullOrWhiteSpace(value)
                && IncidentTitle.IsValidService(value.Trim()))
            .WithMessage("Service must be valid and no longer than 100 characters.");
        RuleFor(command => command.Description)
            .Must(value => !string.IsNullOrWhiteSpace(value)
                && IncidentDescription.IsValidDescription(value.Trim()))
            .WithMessage("Description must contain between 10 and 500 characters.");
        RuleFor(command => command.CategoryId).NotEmpty();
        RuleFor(command => command.Scope)
            .Must(name => IncidentDomainLookups.Scope(name) is not null)
            .WithMessage("Scope must be SingleUser, Department, or Organization.");
        RuleFor(command => command.Environment)
            .Must(name => IncidentDomainLookups.Environment(name) is not null)
            .WithMessage("Environment must be Development, Staging, or Production.");
    }
}
