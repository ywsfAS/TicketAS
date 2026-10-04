namespace Application.Abstractions.Users;

public interface IUserPasswordHasher
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}
