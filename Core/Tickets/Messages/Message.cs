using Core.Utilities;
using Core.Exceptions;

namespace Core.Tickets.Messages
{
    public sealed record MessageId(Guid Id) : StrongTypedId(Id);
    public sealed class Message : Entity<MessageId>
    {
        public MessageContent Content { get; private set; }
        public DateTime SentAt { get; private set; }

        private Message(MessageContent content , DateTime sentAt) => 
            (Content, SentAt) = (content,sentAt);
        public static Message Create(MessageContent content , DateTime sentAt)
        {
            if (content == null) throw new TicketMessageContentIsNullException();
            return new Message(content, sentAt);
        }


    }
}
