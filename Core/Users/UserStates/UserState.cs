namespace Core.Users.UserStates
{
    public abstract class UserState
    {
        public abstract string Name { get; }
        public abstract void ChangeUserName(User user , UserName newUserName);
        public abstract void ChangeEmail(User user , Email newEmail);
        public abstract void ChangePhoneNumber(User user , PhoneNumber phoneNumber);


        public abstract bool CanBeLocked();
        public abstract bool CanBeActivated();
        public abstract bool CanBeDeactivated();

        public abstract override string ToString();
    }
}
