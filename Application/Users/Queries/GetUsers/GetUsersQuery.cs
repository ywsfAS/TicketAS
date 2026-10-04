using Application.Common;
using MediatR;

namespace Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery(
    string? UserName = null,
    string? Email = null,
    string? PhoneNumber = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<UserDto>>;
