using Core.Exceptions;
namespace Core.Tickets.Messages
{
    public sealed record MessageContent
    {
        public MessageTitle Title { get;}
        public MessageBody Body { get;}

        private MessageContent(MessageTitle title, MessageBody body) =>
            (Title, Body) = (title, body);

        public static MessageContent Create(MessageTitle title , MessageBody body)
        {
            if(title == null) throw new TicketMessageTitleIsNullException();
            if(body == null) throw new TicketMessageBodyIsNullException();

            return new MessageContent(title, body);
        }


    }
}
