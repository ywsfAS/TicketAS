using FluentValidation;

namespace Application.Agents.Commands.AddAgentSpecialization;

public sealed class AddAgentSpecializationCommandValidator : AbstractValidator<AddAgentSpecializationCommand>
{
    public AddAgentSpecializationCommandValidator()
    {
        RuleFor(command => command.AgentId).NotEmpty();
        RuleFor(command => command.SpecializationId).NotEmpty();
    }
}
