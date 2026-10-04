using Core.Agents;
using Core.Agents.AgentStates;
using Core.Agents.Seniority;

namespace Application.Agents;

public sealed record AgentSearchCriteria(
    SpecializationId? Specialization,
    AgentSeniority? Seniority,
    AgentState? State,
    int Page,
    int PageSize);
