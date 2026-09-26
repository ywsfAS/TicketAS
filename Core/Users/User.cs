using Core.Exceptions;
using Core.Users.UserStates;
using Core.Utilities;

namespace Core.Users
{
    public sealed record UserId(Guid Id) : StrongTypedId(Id);
    public class User : Entity<UserId>
    {
        public UserName UserName { get; private set; }
        public Email Email { get; private set; }
        public UserPassword Password { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }

        private UserState _state;
        private User() { }

        public static User Create(UserName userName, Email email, PhoneNumber number) =>
            new User
            {
                UserName = userName,
                Email = email,
                PhoneNumber = number,
                _state = new ActiveUser()
            };
        internal void SetUserName(UserName Name) => UserName = Name;
        internal void SetEmail(Email email) => Email = email;
        internal void SetPhoneNumber(PhoneNumber number) => PhoneNumber = number;

        public void ChangeUserName(UserName userName) => _state.ChangeUserName(this,userName); 
        public void ChangeEmail(Email email) => _state.ChangeEmail(this,email);
        public void ChangePhoneNumber(PhoneNumber number) => _state.ChangePhoneNumber(this,number);


        public void Lock()
        {
            if (!_state.CanBeLocked()) throw new UserStateTransitionIsInvalidException(_state.Name,"Locked");
            _state = new LockedUser();
        }
        public void Deactivate()
        {
            if(!_state.CanBeDeactivated()) throw new UserStateTransitionIsInvalidException(_state.Name,"Deactivated");
            _state = new DeactivatedUser();
        }
        public void activate() {
            if (!_state.CanBeActivated()) throw new UserStateTransitionIsInvalidException(_state.Name,"Active");
            _state = new ActiveUser(); 
        }

    }
}
