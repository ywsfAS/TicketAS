using Application.Abstractions.Reporters;
using Application.Common;
using Core.Reporters;
using Core.Reporters.ReportStates;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class ReporterRepository(TicketDbContext dbContext) : IReporterRepository
{
    public async Task AddAsync(Reporter reporter, CancellationToken cancellationToken)
    {
        await dbContext.Reporters.AddAsync(reporter, cancellationToken);
    }

    public Task<Reporter?> GetByIdAsync(ReporterId id, CancellationToken cancellationToken) =>
        dbContext.Reporters.SingleOrDefaultAsync(reporter => reporter.Id == id, cancellationToken);

    public Task<Reporter?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        dbContext.Reporters.SingleOrDefaultAsync(reporter => reporter.UserId == userId, cancellationToken);

    public async Task<PagedResult<Reporter>> SearchAsync(
        ReporterState? state,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var reporters = dbContext.Reporters.AsNoTracking();
        if (state is not null)
            reporters = reporters.Where(reporter => reporter.State == state);

        var totalCount = await reporters.CountAsync(cancellationToken);
        var offset = (int)Math.Min((long)(page - 1) * pageSize, int.MaxValue);
        var items = await reporters
            .OrderBy(reporter => reporter.CreatedAt)
            .ThenBy(reporter => reporter.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<Reporter>.Create(items, page, pageSize, totalCount);
    }
}
