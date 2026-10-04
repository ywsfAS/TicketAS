using Core.Agents.Seniority;
using FluentValidation;

namespace Application.Agents.Commands.CreateAgentProfile;

public sealed class CreateAgentProfileCommandValidator : AbstractValidator<CreateAgentProfileCommand>
{
    public CreateAgentProfileCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Seniority)
            .Must(name => AgentSeniority.FindByName(name) is not null)
            .WithMessage("Seniority must be Junior, Mid, or Senior.");
    }
}
