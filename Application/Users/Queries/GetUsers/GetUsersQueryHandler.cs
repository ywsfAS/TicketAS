using Application.Abstractions.Users;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<IReadOnlyList<UserDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return users.Select(UserDto.From).ToList();
    }
}
