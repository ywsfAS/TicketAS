using MediatR;

namespace Application.Agents.Commands.MakeAgentUnavailable;

public sealed record MakeAgentUnavailableCommand(Guid AgentId) : IRequest<bool>;
