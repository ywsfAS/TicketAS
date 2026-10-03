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

        private readonly List<ConversationAgent> _participants = new();
        public IReadOnlyCollection<ConversationAgent> Participants => _participants;
        public IReadOnlyCollection<AgentId> AgentsIds => _participants.Select(p => p.AgentId).ToList();

        private List<Message> _messages = new List<Message>();
        public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Conversation() { }   // EF Core

        private Conversation(Reporter reporter, DateTime createdAt, DateTime? updatedAt)
        {
            Id = new ConversationId(Guid.NewGuid());
            (Reporter, CreatedAt, UpdatedAt) = (reporter, createdAt, updatedAt);
        }
        public static Conversation Create(Reporter reporter, Agent intialAgent)
        {
            if (reporter == null) throw new TicketConversationReporterIsNullException();
            if (intialAgent == null) throw new TicketConversationAgentIsNullException();

            var conversation = new Conversation(reporter, DateTime.UtcNow, null);
            conversation.AddParticipantInternal(intialAgent.Id);
            return conversation;
        }
        internal void AddParticipant(Agent agent)
        {
            if (agent is null) throw new TicketConversationAgentIsNullException();
            AddParticipantInternal(agent.Id);

            Update();
        }
        public void AgentSends(Agent agent, MessageContent content)
        {
            if (content == null) throw new TicketMessageContentIsNullException();
            if (agent == null) throw new TicketConversationAgentIsNullException();
            if (!_participants.Any(p => p.AgentId == agent.Id)) throw new TicketConversationAgentNotParticipantException();
            var message = Message.Create(agent.Id, content, DateTime.UtcNow);

            this._messages.Add(message);
            Update();
        }
        public void ReporterSends(MessageContent content)
        {
            if (content == null) throw new TicketMessageContentIsNullException();
            var message = Message.Create(Reporter.Id, content, DateTime.UtcNow);

            this._messages.Add(message);

            Update();
        }
        private void AddParticipantInternal(AgentId agentId)
        {
            if (_participants.Any(p => p.AgentId == agentId)) return;   
            _participants.Add(new ConversationAgent(Id, agentId));
        }

        private void Update() => UpdatedAt = DateTime.UtcNow;
    }


}
