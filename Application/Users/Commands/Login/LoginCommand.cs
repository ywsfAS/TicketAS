using Application.Abstractions.Users;
using MediatR;

namespace Application.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResult?>;

public sealed record LoginResult(
    string AccessToken,
    string TokenType,
    DateTimeOffset ExpiresAt,
    UserDto User)
{
    public static LoginResult From(IssuedUserToken token, UserDto user) =>
        new(token.AccessToken, "Bearer", token.ExpiresAt, user);
}
