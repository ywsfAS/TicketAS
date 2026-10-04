using Core.Agents;

namespace Application.Agents;

public sealed record AgentDto(
    Guid Id,
    Guid UserId,
    string Seniority,
    string State,
    IReadOnlyList<Guid> SpecializationIds,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static AgentDto From(Agent agent) =>
        new(
            agent.Id.Id,
            agent.UserId.Id,
            agent.Seniority.Name,
            agent.State.Name,
            agent.Specializations.Select(specialization => specialization.SpecializationId.id).ToList(),
            agent.CreatedAt,
            agent.UpdatedAt);
}
