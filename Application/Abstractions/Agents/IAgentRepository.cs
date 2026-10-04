using Application.Agents;
using Application.Common;
using Core.Agents;
using Core.Users;

namespace Application.Abstractions.Agents;

public interface IAgentRepository
{
    Task AddAsync(Agent agent, CancellationToken cancellationToken);
    Task<Agent?> GetByIdAsync(AgentId id, CancellationToken cancellationToken);
    Task<Agent?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<bool> SpecializationExistsAsync(SpecializationId id, CancellationToken cancellationToken);
    Task<PagedResult<Agent>> SearchAsync(
        AgentSearchCriteria criteria,
        CancellationToken cancellationToken);
}
