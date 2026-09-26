using Core.Agents;
using Core.Exceptions;
using Core.Incidents;
using Core.Reporters;
using Core.Tickets;
using Core.Tickets.Conversation;
using Core.Tickets.TicketPriotities;
using Core.Utilities;

namespace Core
{
    public sealed record TicketId(Guid Value) : StrongTypedId(Value);



    public sealed class Ticket : Entity<TicketId>
    {
        public TicketTitle Name { get; private set; }
        public TicketDescription Description { get; private set; }
        public Reporter Reporter { get; private set; }
        public Agent Agent { get; private set; }
        public Incident Incident { get; private set; }
        public TicketPriority Priority { get; private set; }
        public Conversation Conversation { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Ticket(TicketTitle name, TicketDescription description, Reporter reporter,Agent agent,Incident incident,TicketPriority priority,Conversation conversation, DateTime createdAt, DateTime? updatedAt) =>
            (Name,Description,Reporter,Agent,Incident,Priority,Conversation,CreatedAt,UpdatedAt) 
            = (name, description, reporter, agent,incident, priority, conversation, createdAt, updatedAt);














    }
}
