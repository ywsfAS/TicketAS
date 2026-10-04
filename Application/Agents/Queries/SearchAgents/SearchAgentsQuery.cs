using Application.Common;
using MediatR;

namespace Application.Agents.Queries.SearchAgents;

public sealed record SearchAgentsQuery(
    Guid? SpecializationId = null,
    string? Seniority = null,
    string? State = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<AgentDto>>;
