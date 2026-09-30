using Core.Exceptions;

namespace Core.Users
{
    public sealed record UserName
    {
        public string Name { get; init; }

        private const int MaxLength = 20;
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
            !string.IsNullOrEmpty(userName) && userName.Length >= MinLength && userName.Length <= MaxLength;
    }
}
