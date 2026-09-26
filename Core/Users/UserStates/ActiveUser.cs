namespace Core.Users.UserStates
{
    public sealed class ActiveUser : UserState
    {
        public override string Name { get;} = "Active";
        public override void ChangeEmail(User user, Email newEmail) => user.SetEmail(newEmail);
        public override void ChangePhoneNumber(User user, PhoneNumber phoneNumber) => user.SetPhoneNumber(phoneNumber);
        public override void ChangeUserName(User user, UserName newUserName) => user.SetUserName(newUserName);

        public override bool CanBeLocked() => true;
        public override bool CanBeActivated() => false;
        public override bool CanBeDeactivated() => true;

        public override string ToString() => Name;

    }
}
