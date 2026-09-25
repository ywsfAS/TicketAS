using Core.Exceptions;

namespace Core.Tickets.Messages
{
    public sealed record MessageTitle
    {
        public string Title { get ;}
        private const int MinLength = 3;
        private const int MaxLength = 150;

        private MessageTitle(string title) => Title = title;
        public static MessageTitle Create(string title)
        {
            if(title == null) throw new TicketMessageTitleIsNullException();
            title = title.Trim();
            if (!IsValidTitle(title)) throw new TicketMessageTitleInvalidException(title); 
            return new MessageTitle(title);
        }

        private static bool IsValidTitle(string title) => !String.IsNullOrEmpty(title)
            && title.Length <= MaxLength && title.Length >= MinLength
            && ContainsAlphanumeric(title); 
        private static bool ContainsAlphanumeric(string title) => title.Any(char.IsLetterOrDigit);

    }
}
