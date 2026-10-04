using Application.Abstractions.Agents;
using Core.Agents;
using MediatR;

namespace Application.Agents.Queries.GetAgent;

public sealed class GetAgentQueryHandler(IAgentRepository agentRepository)
    : IRequestHandler<GetAgentQuery, AgentDto?>
{
    public async Task<AgentDto?> Handle(
        GetAgentQuery request,
        CancellationToken cancellationToken)
    {
        var agent = await agentRepository.GetByIdAsync(
            new AgentId(request.AgentId),
            cancellationToken);
        return agent is null ? null : AgentDto.From(agent);
    }
}
