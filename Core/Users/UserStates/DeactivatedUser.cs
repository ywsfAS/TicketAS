using Core.Exceptions;

namespace Core.Users.UserStates
{
    public sealed class DeactivatedUser : UserState
    {
        public override string Name { get;} = "Deactivated";
        public override void ChangeEmail(User user, Email newEmail) => throw new DeactivatedUserException();
        public override void ChangeUserName(User user, UserName newUserName) => throw new DeactivatedUserException();
        public override void ChangePhoneNumber(User user,  PhoneNumber newPhoneNumber) => throw new DeactivatedUserException();

        public override bool CanBeActivated() => true;
        public override bool CanBeLocked() => false;
        public override bool CanBeDeactivated() => false;

        public override string ToString() => Name;

    }
}
