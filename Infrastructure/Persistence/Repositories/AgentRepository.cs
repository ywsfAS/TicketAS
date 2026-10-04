using Application.Abstractions.Agents;
using Application.Agents;
using Application.Common;
using Core.Agents;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class AgentRepository(TicketDbContext dbContext) : IAgentRepository
{
    public async Task AddAsync(Agent agent, CancellationToken cancellationToken)
    {
        await dbContext.Agents.AddAsync(agent, cancellationToken);
    }

    public Task<Agent?> GetByIdAsync(AgentId id, CancellationToken cancellationToken) =>
        dbContext.Agents
            .Include(agent => agent.Specializations)
            .SingleOrDefaultAsync(agent => agent.Id == id, cancellationToken);

    public Task<Agent?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        dbContext.Agents
            .Include(agent => agent.Specializations)
            .SingleOrDefaultAsync(agent => agent.UserId == userId, cancellationToken);

    public Task<bool> SpecializationExistsAsync(
        SpecializationId id,
        CancellationToken cancellationToken) =>
        dbContext.Specializations.AnyAsync(specialization => specialization.Id == id, cancellationToken);

    public async Task<PagedResult<Agent>> SearchAsync(
        AgentSearchCriteria criteria,
        CancellationToken cancellationToken)
    {
        var agents = dbContext.Agents.AsNoTracking();
        if (criteria.Specialization is not null)
            agents = agents.Where(agent =>
                agent.Specializations.Any(item => item.SpecializationId == criteria.Specialization));
        if (criteria.Seniority is not null)
            agents = agents.Where(agent => agent.Seniority == criteria.Seniority);
        if (criteria.State is not null)
            agents = agents.Where(agent => agent.State == criteria.State);

        var totalCount = await agents.CountAsync(cancellationToken);
        var offset = (int)Math.Min((long)(criteria.Page - 1) * criteria.PageSize, int.MaxValue);
        var items = await agents
            .Include(agent => agent.Specializations)
            .OrderBy(agent => agent.CreatedAt)
            .ThenBy(agent => agent.Id)
            .Skip(offset)
            .Take(criteria.PageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<Agent>.Create(items, criteria.Page, criteria.PageSize, totalCount);
    }
}
