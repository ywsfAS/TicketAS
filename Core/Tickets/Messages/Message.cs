using Core.Utilities;
using Core.Exceptions;
namespace Core.Tickets.Messages
{
    public sealed record MessageId(Guid Id) : StrongTypedId(Id);
    public abstract record ConversationParticipantId(Guid Id) : StrongTypedId(Id);
    public sealed class Message : Entity<MessageId>
    {
        public ConversationParticipantId ParticipantId { get; private set; }
        public MessageContent Content { get; private set; }
        public DateTime SentAt { get; private set; }

        private Message() { }
        private Message(ConversationParticipantId id,MessageContent content , DateTime sentAt) => 
            (ParticipantId,Content, SentAt) = (id, content, sentAt);
        public static Message Create(ConversationParticipantId id,MessageContent content , DateTime sentAt)
        {
            if (content == null) throw new TicketMessageContentIsNullException();
            var message =  new Message(id,content, sentAt);
            message.Id = new MessageId(Guid.NewGuid());

            return message;
        }


    }
}
