using Application.Abstractions.Reporters;
using Core.Reporters;
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
}
