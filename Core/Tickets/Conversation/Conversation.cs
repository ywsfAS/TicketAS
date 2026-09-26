using Core.Tickets.Messages;
using Core.Utilities;
using Core.Exceptions;
using Core.Reporters;
using Core.Agents;
namespace Core.Tickets.Conversation
{
    public sealed record ConversationId(Guid Id) : StrongTypedId(Id);
    public sealed class Conversation : Entity<ConversationId>
    {
        public Reporter Reporter { get; private set; }

        private readonly HashSet<AgentId> _agentIds = new();
        public IReadOnlyCollection<AgentId> AgentsIds => _agentIds;

        private List<Message> _messages = new List<Message>();
        public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Conversation(Reporter reporter,DateTime createdAt , DateTime? updatedAt) =>
            (Reporter,CreatedAt,UpdatedAt) = (reporter, createdAt, updatedAt);
        public static Conversation Create(Reporter reporter,Agent intialAgent)
        {
            if(reporter == null) throw new TicketConversationReporterIsNullException();

            var conversation = new Conversation(reporter,DateTime.UtcNow,null);
            conversation._agentIds.Add(intialAgent.Id);
            return conversation;
        }
        internal void AddParticipant(Agent agent)
        {
            if (agent is null) throw new TicketConversationAgentIsNullException();
            _agentIds.Add(agent.Id);

            Update();
        }
        public void AgentSends(Agent agent,MessageContent content)
        {
            if(content == null) throw new TicketMessageContentIsNullException();
            if (agent == null) throw new AgentIsNullException();
            if(!_agentIds.Contains(agent.Id)) throw new TicketConversationAgentNotParticipantException();
            var message = Message.Create(agent.Id,content,DateTime.UtcNow);

            this._messages.Add(message);
            Update();
        }
        public void ReporterSends(MessageContent content)
        {
            if(content == null) throw new TicketMessageContentIsNullException();
            var message = Message.Create(Reporter.Id,content,DateTime.UtcNow);

            this._messages.Add(message);

            Update();
        }
        private void Update() => UpdatedAt = DateTime.UtcNow;   
    }


}
