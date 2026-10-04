using Application.Abstractions.Users;
using Application.Common;
using Core.Users;
using Core.Users.UserStates;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await userRepository.SearchAsync(
            new UserSearchCriteria(
                string.IsNullOrWhiteSpace(request.UserName) ? null : UserName.Create(request.UserName),
                string.IsNullOrWhiteSpace(request.Email) ? null : Email.Create(request.Email),
                string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : PhoneNumber.Create(request.PhoneNumber),
                UserStateFor(request.State)),
            request.Page,
            request.PageSize,
            cancellationToken);

        return PagedResult<UserDto>.Create(
            result.Items.Select(UserDto.From).ToList(),
            result.Page,
            result.PageSize,
            result.TotalCount);
    }

    private static UserState? UserStateFor(string? state) => state switch
    {
        null or "" => null,
        "Active" => new ActiveUser(),
        "Locked" => new LockedUser(),
        "Deactivated" => new DeactivatedUser(),
        _ => throw new InvalidOperationException("User state must be validated before handling the query.")
    };
}
