using Core.Users;

namespace Application.Abstractions.Users;

public interface IUserTokenIssuer
{
    IssuedUserToken Issue(User user);
}

public sealed record IssuedUserToken(string AccessToken, DateTimeOffset ExpiresAt);
