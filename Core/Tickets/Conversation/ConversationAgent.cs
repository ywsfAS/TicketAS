using Core.Agents;

namespace Core.Tickets.Conversation
{

    public sealed class ConversationAgent
    {
        public ConversationId ConversationId { get; private set; }
        public AgentId AgentId { get; private set; }

        private ConversationAgent() { }

        public ConversationAgent(
            ConversationId conversationId,
            AgentId agentId)
        {
            ConversationId = conversationId;
            AgentId = agentId;
        }
    }
}
