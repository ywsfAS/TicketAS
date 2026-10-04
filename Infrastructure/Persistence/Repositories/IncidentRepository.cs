using Application.Abstractions.Incidents;
using Application.Common;
using Application.Incidents;
using Core.Incidents;
using Core.Incidents.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class IncidentRepository(TicketDbContext dbContext) : IIncidentRepository
{
    public async Task AddAsync(Incident incident, CancellationToken cancellationToken)
    {
        await dbContext.Incidents.AddAsync(incident, cancellationToken);
    }

    public Task<Incident?> GetByIdAsync(IncidentId id, CancellationToken cancellationToken) =>
        dbContext.Incidents
            .Include(incident => incident.Category)
            .SingleOrDefaultAsync(incident => incident.Id == id, cancellationToken);

    public Task<IncidentCategory?> GetCategoryByIdAsync(
        IncidentCategoryId id,
        CancellationToken cancellationToken) =>
        dbContext.IncidentCategories.SingleOrDefaultAsync(category => category.Id == id, cancellationToken);

    public async Task<IIncidentCategoryBehavior?> GetCategoryBehaviorByIdAsync(
        IncidentCategoryId id,
        CancellationToken cancellationToken) =>
        await dbContext.NetworkIncidentCategories
            .Include(networkCategory => networkCategory.Category)
            .SingleOrDefaultAsync(networkCategory => networkCategory.CategoryId == id, cancellationToken);

    public async Task<PagedResult<Incident>> SearchAsync(
        IncidentSearchCriteria criteria,
        CancellationToken cancellationToken)
    {
        var incidents = dbContext.Incidents
            .AsNoTracking()
            .Include(incident => incident.Category)
            .AsQueryable();

        if (criteria.ReporterId is not null)
            incidents = incidents.Where(incident => incident.ReporterId == criteria.ReporterId);
        if (criteria.CategoryId is not null)
            incidents = incidents.Where(incident => incident.CategoryId == criteria.CategoryId);
        if (criteria.Severity is not null)
            incidents = incidents.Where(incident => incident.IncidentSeverity == criteria.Severity);
        if (criteria.Environment is not null)
            incidents = incidents.Where(incident => incident.Environment == criteria.Environment);
        if (criteria.IsOverdue is true)
            incidents = incidents.Where(incident =>
                incident.IncidentSla.Resolution.DueAt < DateTime.UtcNow);
        else if (criteria.IsOverdue is false)
            incidents = incidents.Where(incident =>
                incident.IncidentSla.Resolution.DueAt >= DateTime.UtcNow);

        var totalCount = await incidents.CountAsync(cancellationToken);
        var offset = (int)Math.Min((long)(criteria.Page - 1) * criteria.PageSize, int.MaxValue);
        var items = await incidents
            .OrderByDescending(incident => incident.CreatedAt)
            .ThenBy(incident => incident.Id)
            .Skip(offset)
            .Take(criteria.PageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<Incident>.Create(items, criteria.Page, criteria.PageSize, totalCount);
    }
}
