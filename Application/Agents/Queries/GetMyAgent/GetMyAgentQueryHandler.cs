using Application.Abstractions.Agents;
using Core.Users;
using MediatR;

namespace Application.Agents.Queries.GetMyAgent;

public sealed class GetMyAgentQueryHandler(IAgentRepository agentRepository)
    : IRequestHandler<GetMyAgentQuery, AgentDto?>
{
    public async Task<AgentDto?> Handle(
        GetMyAgentQuery request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByUserIdAsync(
            new UserId(request.UserId),
            cancellationToken);
        return agent is null ? null : AgentDto.From(agent);
    }
}
