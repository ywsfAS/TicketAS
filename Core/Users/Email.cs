using Core.Exceptions;
using System.Text.RegularExpressions;

namespace Core.Users
{
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

        public static bool IsValidEmail(string email) => !string.IsNullOrEmpty(email) && Regex.IsMatch(email,"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");


    };
}
