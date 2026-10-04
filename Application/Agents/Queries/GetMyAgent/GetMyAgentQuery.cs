using MediatR;

namespace Application.Agents.Queries.GetMyAgent;

public sealed record GetMyAgentQuery(Guid UserId) : IRequest<AgentDto?>;
