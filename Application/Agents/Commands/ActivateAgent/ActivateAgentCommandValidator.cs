using FluentValidation;

namespace Application.Agents.Commands.ActivateAgent;

public sealed class ActivateAgentCommandValidator : AbstractValidator<ActivateAgentCommand>
{
    public ActivateAgentCommandValidator() =>
        RuleFor(command => command.AgentId).NotEmpty();
}
