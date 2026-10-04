using Application.Abstractions.Users;
using Core.Users;
using MediatR;

namespace Application.Users.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IUserPasswordHasher passwordHasher,
    IUserTokenIssuer tokenIssuer) : IRequestHandler<LoginCommand, LoginResult?>
{
    public async Task<LoginResult?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(
            Email.Create(request.Email),
            cancellationToken);

        if (user?.Password is null
            || user.StateName != "Active"
            || !passwordHasher.Verify(user.Password.Password, request.Password))
        {
            return null;
        }

        var token = tokenIssuer.Issue(user);
        return LoginResult.From(token, UserDto.From(user));
    }
}
