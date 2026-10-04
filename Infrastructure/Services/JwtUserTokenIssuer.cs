using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Users;
using Core.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public sealed class JwtUserTokenIssuer(IOptions<JwtOptions> options) : IUserTokenIssuer
{
    public IssuedUserToken Issue(User user)
    {
        var jwtOptions = options.Value;
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(jwtOptions.AccessTokenMinutes);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Address),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName.Name)
        };

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new IssuedUserToken(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt);
    }
}
