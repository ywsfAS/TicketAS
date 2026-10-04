using Application.Incidents;
using Application.Common;
using Core.Incidents;
using Core.Incidents.Categories;

namespace Application.Abstractions.Incidents;

public interface IIncidentRepository
{
    Task AddAsync(Incident incident, CancellationToken cancellationToken);
    Task<Incident?> GetByIdAsync(IncidentId id, CancellationToken cancellationToken);
    Task<IncidentCategory?> GetCategoryByIdAsync(
        IncidentCategoryId id,
        CancellationToken cancellationToken);
    Task<IIncidentCategoryBehavior?> GetCategoryBehaviorByIdAsync(
        IncidentCategoryId id,
        CancellationToken cancellationToken);
    Task<PagedResult<Incident>> SearchAsync(
        IncidentSearchCriteria criteria,
        CancellationToken cancellationToken);
}
