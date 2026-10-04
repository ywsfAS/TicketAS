using Application.Abstractions.Users;
using Core.Users;
using MediatR;

namespace Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(new UserId(request.Id), cancellationToken);
        return user is null ? null : UserDto.From(user);
    }
}
