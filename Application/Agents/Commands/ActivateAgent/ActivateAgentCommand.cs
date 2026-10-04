using MediatR;

namespace Application.Agents.Commands.ActivateAgent;

public sealed record ActivateAgentCommand(Guid AgentId) : IRequest<bool>;
