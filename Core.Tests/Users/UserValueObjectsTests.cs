using Core.Exceptions;
using Core.Users;


namespace Core.Tests.Users
{
        public class UserNameTests
        {
            [Theory]
            [InlineData("Al")]
            [InlineData("Jonathan Doe")]
            [InlineData("  Trimmed  ")]
            public void Create_WithValidName_Succeeds(string name)
            {
                var userName = UserName.Create(name);

                Assert.Equal(name.Trim(), userName.Name);
            }

            [Theory]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("A")]
            [InlineData(null)]
            public void Create_WithInvalidName_Throws(string? name)
            {
                Assert.Throws<UserNameException>(() => UserName.Create(name!));
            }
        }

        public class EmailTests
        {
            [Theory]
            [InlineData("john@example.com")]
            [InlineData("john.doe+tag@sub.example.co")]
            public void Create_WithValidEmail_Succeeds(string email)
            {
                var result = Email.Create(email);

                Assert.Equal(email, result.Address);
            }

            [Theory]
            [InlineData("")]
            [InlineData("not-an-email")]
            [InlineData("missing-domain@")]
            [InlineData("@missing-local.com")]
            [InlineData(null)]
            public void Create_WithInvalidEmail_Throws(string? email)
            {
                Assert.Throws<UserEmailException>(() => Email.Create(email!));
            }
        }

        public class PhoneNumberTests
        {
            [Theory]
            [InlineData("+1234567890")]
            [InlineData("123456789")]
            public void Create_WithValidPhoneNumber_Succeeds(string phone)
            {
                var result = PhoneNumber.Create(phone);

                Assert.Equal(phone, result.Phone);
            }

            [Theory]
            [InlineData("")]
            [InlineData("12345")]
            [InlineData("abcdefghij")]
            [InlineData(null)]
            public void Create_WithInvalidPhoneNumber_Throws(string? phone)
            {
                Assert.Throws<UserPhoneNumberException>(() => PhoneNumber.Create(phone!));
            }
        }

        public class UserPasswordTests
        {
            [Theory]
            [InlineData("12345678")]
            [InlineData("aValidPassword123")]
            public void Create_WithValidPassword_Succeeds(string password)
            {
                var result = UserPassword.Create(password);

                Assert.Equal(password, result.Password);
            }

            [Theory]
            [InlineData("")]
            [InlineData("short")]
            [InlineData(null)]
            public void Create_WithInvalidPassword_Throws(string? password)
            {
                Assert.Throws<UserPasswordException>(() => UserPassword.Create(password!));
            }

            [Fact]
            public void Create_WithPasswordLongerThanMax_Throws()
            {
                var tooLong = new string('a', 501);

                Assert.Throws<UserPasswordException>(() => UserPassword.Create(tooLong));
            }
        }
}
