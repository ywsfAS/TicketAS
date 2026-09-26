
namespace Core.Tickets.TicketPriotities
{
    public sealed class HighTicket : TicketPriority
    {
        public override int Level { get; } = 2;

        public override bool RequiresImmediateAssignment() => false;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => false;
        public override bool AllowsWaitInQueue() => false;

    }
}
