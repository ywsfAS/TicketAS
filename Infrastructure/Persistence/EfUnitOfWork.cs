using Application.Abstractions.Persistence;

namespace Infrastructure.Persistence;

public sealed class EfUnitOfWork(TicketDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
