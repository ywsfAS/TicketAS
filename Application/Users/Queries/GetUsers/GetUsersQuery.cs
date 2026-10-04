using MediatR;

namespace Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;
