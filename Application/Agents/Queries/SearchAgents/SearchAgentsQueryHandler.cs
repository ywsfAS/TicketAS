using Application.Abstractions.Agents;
using Application.Agents;
using Application.Common;
using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;
using MediatR;

namespace Application.Agents.Queries.SearchAgents;

public sealed class SearchAgentsQueryHandler(IAgentRepository agentRepository)
    : IRequestHandler<SearchAgentsQuery, PagedResult<AgentDto>>
{
    public async Task<PagedResult<AgentDto>> Handle(
        SearchAgentsQuery request,
        CancellationToken cancellationToken)
    {
        var criteria = new AgentSearchCriteria(
            request.SpecializationId is Guid specializationId
                ? new SpecializationId(specializationId)
                : null,
            string.IsNullOrWhiteSpace(request.Seniority)
                ? null
                : AgentSeniority.FindByName(request.Seniority),
            AgentStateFor(request.State),
            request.Page,
            request.PageSize);
        var result = await agentRepository.SearchAsync(criteria, cancellationToken);

        return PagedResult<AgentDto>.Create(
            result.Items.Select(AgentDto.From).ToList(),
            result.Page,
            result.PageSize,
            result.TotalCount);
    }

    private static AgentState? AgentStateFor(string? state) => state switch
    {
        null or "" => null,
        "Active" => new ActiveAgentState(),
        "Unavailable" => new UnavailableAgentState(),
        "Suspended" => new SuspendedAgentState(),
        _ => throw new InvalidOperationException("Agent state must be validated before handling the query.")
    };
}
