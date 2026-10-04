using Application.Abstractions.Users;
using Application.Common;
using Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class UserRepository(TicketDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        if (user.Password is null)
            throw new InvalidOperationException("A password hash is required before a user can be persisted.");

        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken) =>
        dbContext.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken) =>
        dbContext.Users.SingleOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<PagedResult<User>> SearchAsync(
        UserSearchCriteria criteria,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var users = dbContext.Users.AsNoTracking();

        if (criteria.UserName is not null)
            users = users.Where(user => user.UserName == criteria.UserName);

        if (criteria.Email is not null)
            users = users.Where(user => user.Email == criteria.Email);

        if (criteria.PhoneNumber is not null)
            users = users.Where(user => user.PhoneNumber == criteria.PhoneNumber);

        var totalCount = await users.CountAsync(cancellationToken);
        var offset = (int)Math.Min((long)(page - 1) * pageSize, int.MaxValue);
        var items = await users
            .OrderBy(user => user.CreatedAt)
            .ThenBy(user => user.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<User>.Create(items, page, pageSize, totalCount);
    }
}
