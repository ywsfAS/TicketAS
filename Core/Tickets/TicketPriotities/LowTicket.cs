namespace Core.Tickets.TicketPriotities
{
    public sealed class LowTicket : TicketPriority { 
        public override int Level {get;} = 0;
        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => true;
        public override bool AllowsWaitInQueue() => true;

    
    }
}
