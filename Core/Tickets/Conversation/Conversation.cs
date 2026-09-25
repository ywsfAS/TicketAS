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

        private Conversation(Reporter reporter, Agent agent) =>
            (Reporter, Agent) = (reporter, agent);
        public static Conversation Create(Reporter reporter , Agent agent)
        {
            if(reporter == null) throw new TicketConversationReporterIsNullException();
            if(agent == null) throw new TicketConversationAgentIsNullException();

            return new Conversation(reporter, agent);
        }
    }
}
