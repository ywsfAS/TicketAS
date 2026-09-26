
using Core.Tickets.Messages;
using Core.Utilities;

namespace Core
{
    public sealed record AgentId(Guid Id) : ConversationParticipantId(Id);
    public class Agent : Entity<AgentId>
    {
    }
}
