using Core.Exceptions;
using Core.Utilities;
using System.Text.RegularExpressions;

namespace Core
{
    public sealed record UserId(Guid Id) : StrongTypedId(Id);
    public sealed record UserName
    {
        public string Name { get; init; }

        private const int MinLength = 2;
        private UserName() { }
        public static UserName Create(string name)
        {
            if(!IsValidUserName(name)) throw new UserNameException(name);

            var Name = name.Trim();
            return new UserName
            {
                Name = Name,
            };
        }
        public static bool IsValidUserName(string userName) => 
            !String.IsNullOrEmpty(userName) && userName.Length >= MinLength ;
    }
    public sealed record PhoneNumber
    {
        public string Phone { get; init; }
        private PhoneNumber() { }

        public static PhoneNumber Create(string phone)
        {
            if(!IsValidPhoneNumber(phone)) throw new UserPhoneNumberException(phone);

            var Number = phone.Trim();
            return new PhoneNumber
            {
                Phone = Number,
            };
        }

        public static bool IsValidPhoneNumber(string number) => !String.IsNullOrEmpty(number) && Regex.IsMatch(number, @"^\+?[0-9]{9,15}$");
    };
    public sealed record UserPassword
    {
        public string Password { get; init; }

        private const int MinLength = 8;

        private const int MaxLength = 50;

        private UserPassword() { }

        public static UserPassword Create(string password)
        {
            if(!IsValidPassword(password)) throw new UserPasswordException(password);

            var Password = password.Trim();
            return new UserPassword
            {
                Password = Password,
            };

        }

         public static bool IsValidPassword(string password) => !String.IsNullOrEmpty(password) && password.Length >= MinLength && password.Length <= MaxLength;
    }
    public sealed record Email
    {
        public string Address { get; init; }

        private Email() { }

        public static Email Create(string address) 
        { 
            if(!IsValidEmail(address)) throw new UserEmailException(address);

            var Email = address.Trim();
            return new Email
            {
                Address = Email
            };
        }

        public static bool IsValidEmail(string email) => !String.IsNullOrEmpty(email) && Regex.IsMatch(email,"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");


    };

    public abstract class UserState
    {
        public abstract void ChangeUserName(User user , UserName newUserName);
        public abstract void ChangeEmail(User user , Email newEmail);
        public abstract void ChangePhoneNumber(User user , PhoneNumber phoneNumber);
    }
    public sealed class ActiveUser : UserState
    {
        public override void ChangeEmail(User user, Email newEmail) => user.SetEmail(newEmail);
        public override void ChangePhoneNumber(User user, PhoneNumber phoneNumber) => user.SetPhoneNumber(phoneNumber);
        public override void ChangeUserName(User user, UserName newUserName) => user.SetUserName(newUserName);

    }
    public sealed class LockedUser : UserState
    {
        public override void ChangeEmail(User user, Email newEmail) => throw new LockedUserException();
        public override void ChangeUserName(User user, UserName newUserName) => throw new LockedUserException();
        public override void ChangePhoneNumber(User user,  PhoneNumber newPhoneNumber) => throw new LockedUserException();

    }
    public sealed class DeactivatedUser : UserState
    {
        public override void ChangeEmail(User user, Email newEmail) => throw new DeactivatedUserException();
        public override void ChangeUserName(User user, UserName newUserName) => throw new DeactivatedUserException();
        public override void ChangePhoneNumber(User user,  PhoneNumber newPhoneNumber) => throw new DeactivatedUserException();
    }

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
        public void SetUserName(UserName Name) => this.UserName = Name;
        public void SetEmail(Email email) => this.Email = email;
        public void SetPhoneNumber(PhoneNumber number) => this.PhoneNumber = number;

        public void ChangeUserName(UserName userName) => this._state.ChangeUserName(this,userName); 
        public void ChangeEmail(Email email) => this._state.ChangeEmail(this,email);
        public void ChangePhoneNumber(PhoneNumber number) => this._state.ChangePhoneNumber(this,number);


        public void Lock() => this._state = new LockedUser();
        public void Unlock() => this._state = new ActiveUser();
        public void Deactivate() => this._state = new DeactivatedUser();
        public void activate() => this._state = new ActiveUser();



    }
}
