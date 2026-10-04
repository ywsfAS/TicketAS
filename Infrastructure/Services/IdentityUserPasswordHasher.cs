using Application.Abstractions.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public sealed class IdentityUserPasswordHasher : IUserPasswordHasher
{
    private static readonly object PasswordHashSubject = new();
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password) =>
        _passwordHasher.HashPassword(PasswordHashSubject, password);

    public bool Verify(string hashedPassword, string providedPassword) =>
        _passwordHasher.VerifyHashedPassword(PasswordHashSubject, hashedPassword, providedPassword)
        is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
}
