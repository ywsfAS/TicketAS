using Application.Common;
using Core.Users;
using Core.Users.UserStates;

namespace Application.Abstractions.Users;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<PagedResult<User>> SearchAsync(
        UserSearchCriteria criteria,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
