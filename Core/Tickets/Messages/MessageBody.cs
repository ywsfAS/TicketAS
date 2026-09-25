using Core.Exceptions;
namespace Core.Tickets.Messages
{
    public sealed record MessageBody
    {
        public string Body { get ;}
        private const int MinLength = 1;
        private const int MaxLength = 5000;

        private MessageBody(string body) => Body = body;
        public static MessageBody Create(string body)
        {
            if(body == null) throw new TicketMessageBodyIsNullException();
            body = body.Trim();
            if (!IsValidTitle(body)) throw new TicketMessageBodyInvalidException(body); 
            return new MessageBody(body);
        }

        private static bool IsValidTitle(string body) => !String.IsNullOrEmpty(body)
            && body.Length <= MaxLength && body.Length >= MinLength
            && ContainsAlphanumeric(body); 
        private static bool ContainsAlphanumeric(string body) => body.Any(char.IsLetterOrDigit);

    }
}
