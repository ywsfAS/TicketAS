using Core.Exceptions;

namespace Core.Users.UserStates
{
    public sealed class LockedUser : UserState
    {
        public override string Name { get;} = "Locked";
        public override void ChangeEmail(User user, Email newEmail) => throw new LockedUserException();
        public override void ChangeUserName(User user, UserName newUserName) => throw new LockedUserException();
        public override void ChangePhoneNumber(User user,  PhoneNumber newPhoneNumber) => throw new LockedUserException();

        public override bool CanBeDeactivated() => true;
        public override bool CanBeActivated() => true;
        public override bool CanBeLocked() => false;

        public override string ToString() => Name;

    }
}
