using FluentValidation;

namespace Application.Agents.Commands.SuspendAgent;

public sealed class SuspendAgentCommandValidator : AbstractValidator<SuspendAgentCommand>
{
    public SuspendAgentCommandValidator() =>
        RuleFor(command => command.AgentId).NotEmpty();
}
