
namespace Core.Tickets.TicketPriotities
{
    public sealed class NormalTicket : TicketPriority {

        public override int Level { get; } = 1;
        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => false;
        public override bool AllowsUnassigned() => true;
        public override bool AllowsWaitInQueue() => false;
    
    }
}
