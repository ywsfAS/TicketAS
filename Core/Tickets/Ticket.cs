using Core.Exceptions;
using Core.Incidents;
using Core.Tickets;
using Core.Tickets.Conversation;
using Core.Utilities;

namespace Core
{
    public sealed record TicketId(Guid Value) : StrongTypedId(Value);

    public abstract class TicketPriority { 
        public abstract int Level { get;}
        public abstract bool AllowsUnassigned();
        public abstract bool AllowsWaitInQueue();
        public abstract bool RequiresAssignment();
        public abstract bool RequiresImmediateAssignment();
    }
    public sealed class CriticalTicket : TicketPriority {
        public override int Level { get; } = 3;

        public override bool RequiresImmediateAssignment() => true;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => false;
        public override bool AllowsWaitInQueue() => false;
    }

    public sealed class HighTicket : TicketPriority
    {
        public override int Level { get; } = 2;

        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => false;
        public override bool AllowsWaitInQueue() => false;

    }
    public sealed class NormalTicket : TicketPriority {

        public override int Level { get; } = 1;
        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => false;
        public override bool AllowsUnassigned() => true;
        public override bool AllowsWaitInQueue() => false;
    
    }
    public sealed class LowTicket : TicketPriority { 
        public override int Level {get;} = 0;
        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => true;
        public override bool AllowsWaitInQueue() => true;

    
    }

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
