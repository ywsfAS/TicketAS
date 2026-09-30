using Core.Exceptions;
using System.Text.RegularExpressions;

namespace Core.Users
{
    public sealed record PhoneNumber
    {
        private const int MaxLength = 16;
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

        public static bool IsValidPhoneNumber(string number) => !string.IsNullOrEmpty(number)&& number.Length <= MaxLength && Regex.IsMatch(number, @"^\+?[0-9]{9,15}$");
    };
}
