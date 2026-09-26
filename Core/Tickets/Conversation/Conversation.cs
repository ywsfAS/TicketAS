using Core.Tickets.Messages;
using Core.Utilities;
using Core.Exceptions;
namespace Core.Tickets.Conversation
{
    public sealed record ConversationId(Guid Id) : StrongTypedId(Id);
    public sealed class Conversation : Entity<ConversationId>
    {
        public Reporter Reporter { get; private set; }
        public Agent Agent { get; private set; }

        private List<Message> _messages = new List<Message>();
        public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Conversation(Reporter reporter, Agent agent,DateTime createdAt , DateTime? updatedAt) =>
            (Reporter, Agent,CreatedAt,UpdatedAt) = (reporter, agent,createdAt,updatedAt);
        public static Conversation Create(Reporter reporter , Agent agent)
        {
            if(reporter == null) throw new TicketConversationReporterIsNullException();
            if(agent == null) throw new TicketConversationAgentIsNullException();

            return new Conversation(reporter,agent,DateTime.UtcNow,null);
        }
        public void AgentSends(MessageContent content)
        {
            if(content == null) throw new TicketMessageContentIsNullException();
            var message = Message.Create(Agent.Id,content,DateTime.UtcNow);

            this._messages.Add(message);
        }
        public void ReporterSends(MessageContent content)
        {
            if(content == null) throw new TicketMessageContentIsNullException();
            var message = Message.Create(Reporter.Id,content,DateTime.UtcNow);

            this._messages.Add(message);
        }
    }
}
