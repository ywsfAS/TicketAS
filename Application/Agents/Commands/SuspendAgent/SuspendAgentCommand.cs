using MediatR;

namespace Application.Agents.Commands.SuspendAgent;

public sealed record SuspendAgentCommand(Guid AgentId) : IRequest<bool>;
