using Core.Exceptions;
using Core.Users;

namespace Core.Tests.Users
{
    public class UserTests
    {
        private static User AUser() =>
            User.Create(
                UserName.Create("John Doe"),
                Email.Create("john@example.com"),
                PhoneNumber.Create("+1234567890"));

        [Fact]
        public void Create_ANewUser_HasSuppliedDetails()
        {
            var userName = UserName.Create("John Doe");
            var email = Email.Create("john@example.com");
            var phone = PhoneNumber.Create("+1234567890");

            var user = User.Create(userName, email, phone);

            Assert.Equal(userName, user.UserName);
            Assert.Equal(email, user.Email);
            Assert.Equal(phone, user.PhoneNumber);
        }

        [Fact]
        public void ChangeEmail_WhileActive_Succeeds()
        {
            var user = AUser();
            var newEmail = Email.Create("new@example.com");

            user.ChangeEmail(newEmail);

            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void Lock_ThenChangeEmail_Throws()
        {
            var user = AUser();
            user.Lock();

            Assert.Throws<LockedUserException>(() => user.ChangeEmail(Email.Create("new@example.com")));
        }

        [Fact]
        public void Deactivate_ThenChangeUserName_Throws()
        {
            var user = AUser();
            user.Deactivate();

            Assert.Throws<DeactivatedUserException>(() => user.ChangeUserName(UserName.Create("New Name")));
        }

        [Fact]
        public void Lock_TwiceInARow_Throws()
        {
            var user = AUser();
            user.Lock();

            Assert.Throws<UserStateTransitionIsInvalidException>(() => user.Lock());
        }

        [Fact]
        public void Deactivate_ThenActivate_ReturnsToActiveState()
        {
            var user = AUser();
            user.Deactivate();

            user.activate();

            user.ChangeEmail(Email.Create("back-to-active@example.com"));
            Assert.Equal("back-to-active@example.com", user.Email.Address);
        }

        [Fact]
        public void Locked_CanBeActivatedDirectly()
        {
            var user = AUser();
            user.Lock();

            user.activate();

            user.ChangeEmail(Email.Create("unlocked@example.com"));
            Assert.Equal("unlocked@example.com", user.Email.Address);
        }
    }
}
