
namespace Core.Tickets.TicketPriotities
{
    public sealed class CriticalTicket : TicketPriority {
        public override int Level { get; } = 3;

        public override bool RequiresImmediateAssignment() => true;
        public override bool RequiresAssignment() => true;
        public override bool AllowsUnassigned() => false;
        public override bool AllowsWaitInQueue() => false;
    }
}
