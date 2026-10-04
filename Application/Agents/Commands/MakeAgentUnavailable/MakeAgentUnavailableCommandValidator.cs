using FluentValidation;

namespace Application.Agents.Commands.MakeAgentUnavailable;

public sealed class MakeAgentUnavailableCommandValidator : AbstractValidator<MakeAgentUnavailableCommand>
{
    public MakeAgentUnavailableCommandValidator() =>
        RuleFor(command => command.AgentId).NotEmpty();
}
