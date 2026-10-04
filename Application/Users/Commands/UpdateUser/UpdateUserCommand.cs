using MediatR;

namespace Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string UserName,
    string Email,
    string PhoneNumber) : IRequest<UserDto?>;
