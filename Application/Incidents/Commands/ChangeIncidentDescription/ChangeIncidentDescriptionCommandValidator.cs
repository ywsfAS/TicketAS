using Core.Incidents;
using FluentValidation;

namespace Application.Incidents.Commands.ChangeIncidentDescription;

public sealed class ChangeIncidentDescriptionCommandValidator : AbstractValidator<ChangeIncidentDescriptionCommand>
{
    public ChangeIncidentDescriptionCommandValidator()
    {
        RuleFor(command => command.IncidentId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Text)
            .Must(value => !string.IsNullOrWhiteSpace(value)
                && IncidentDescription.IsValidDescription(value.Trim()))
            .WithMessage("Description must contain between 10 and 500 characters.");
    }
}
