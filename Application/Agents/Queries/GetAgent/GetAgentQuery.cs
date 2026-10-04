using MediatR;

namespace Application.Agents.Queries.GetAgent;

public sealed record GetAgentQuery(Guid AgentId) : IRequest<AgentDto?>;
