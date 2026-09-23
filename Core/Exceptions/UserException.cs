
namespace Core.Exceptions
{
    public class LockedUserException() : DomainException("The user account is locked and cannot make this operation");
    public class DeactivatedUserException() : DomainException("The user account is deactivated and cannot make this operation");
    public class UserNameException(string Name) : DomainException($"Username is invalid : {Name}");
    public class UserPhoneNumberException(string Phone) : DomainException($"Phone number is invalid : {Phone}");
    public class UserEmailException(string Email) : DomainException($"Email is invalid : {Email}");
    public class UserPasswordException(string Password) : DomainException($"Password is invalid : {Password}");
}
