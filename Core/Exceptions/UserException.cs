
namespace Core.Exceptions
{
    public sealed class LockedUserException() : DomainException("The user account is locked and cannot make this operation");
    public sealed class DeactivatedUserException() : DomainException("The user account is deactivated and cannot make this operation");
    public sealed class UserNameException(string Name) : DomainException($"Username is invalid : {Name}");
    public sealed class UserPhoneNumberException(string Phone) : DomainException($"Phone number is invalid : {Phone}");
    public sealed class UserEmailException(string Email) : DomainException($"Email is invalid : {Email}");
    public sealed class UserPasswordException(string Password) : DomainException($"Password is invalid : {Password}");
    public sealed class UserStateTransitionIsInvalidException(string from,string to) : DomainException($"Invalid transition : from {from} => {to} ");


}
