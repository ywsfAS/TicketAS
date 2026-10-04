using FluentValidation;

namespace Application.Agents.Commands.RemoveAgentSpecialization;

public sealed class RemoveAgentSpecializationCommandValidator : AbstractValidator<RemoveAgentSpecializationCommand>
{
    public RemoveAgentSpecializationCommandValidator()
    {
        RuleFor(command => command.AgentId).NotEmpty();
        RuleFor(command => command.AgentSpecializationId).NotEmpty();
    }
}
