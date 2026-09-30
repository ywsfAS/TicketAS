using Core.Exceptions;

namespace Core.Users
{
    public sealed record UserPassword
    {
        public string Password { get; init; }

        private const int MinLength = 8;

        private const int MaxLength = 500;

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

         public static bool IsValidPassword(string password) => !string.IsNullOrEmpty(password) && password.Length >= MinLength && password.Length <= MaxLength;
    }
}
