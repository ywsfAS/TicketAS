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
        public UserPassword? Password { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string StateName => _state.Name;

        private UserState _state;

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        private User() { }

        public static User Create(UserName userName, Email email, PhoneNumber number) =>
            new User
            {
                Id = new UserId(Guid.NewGuid()),
                UserName = userName,
                Email = email,
                PhoneNumber = number,
                _state = new ActiveUser(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

        public static User Create(UserName userName, Email email, PhoneNumber number, UserPassword password) =>
            new User
            {
                Id = new UserId(Guid.NewGuid()),
                UserName = userName,
                Email = email,
                Password = password,
                PhoneNumber = number,
                _state = new ActiveUser(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
        internal void SetUserName(UserName Name) => UserName = Name;
        internal void SetEmail(Email email) => Email = email;
        internal void SetPhoneNumber(PhoneNumber number) => PhoneNumber = number;

        public void ChangeUserName(UserName userName) {
            _state.ChangeUserName(this, userName);
            Update();
        }

        public void ChangeEmail(Email email) {
            _state.ChangeEmail(this, email);
            Update();
        }
        public void ChangePhoneNumber(PhoneNumber number) {
            _state.ChangePhoneNumber(this, number);
            Update();
                
         }

        public void Lock()
        {
            if (!_state.CanBeLocked()) throw new UserStateTransitionIsInvalidException(_state.Name,"Locked");
            _state = new LockedUser();
            Update();
        }
        public void Deactivate()
        {
            if(!_state.CanBeDeactivated()) throw new UserStateTransitionIsInvalidException(_state.Name,"Deactivated");
            _state = new DeactivatedUser();
            Update();
        }
        public void activate() {
            if (!_state.CanBeActivated()) throw new UserStateTransitionIsInvalidException(_state.Name,"Active");
            _state = new ActiveUser();
            Update();
        }

        protected void Update() => UpdatedAt = DateTime.UtcNow; 
    }
}
