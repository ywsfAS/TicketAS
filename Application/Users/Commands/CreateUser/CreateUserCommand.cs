using MediatR;

namespace Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string UserName,
    string Email,
    string PhoneNumber,
    string Password) : IRequest<UserDto>;
