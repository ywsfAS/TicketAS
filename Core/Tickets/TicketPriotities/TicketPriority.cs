
namespace Core.Tickets.TicketPriotities
{
    public abstract class TicketPriority { 
        public abstract int Level { get;}
        public abstract bool AllowsUnassigned();
        public abstract bool AllowsWaitInQueue();
        public abstract bool RequiresAssignment();
        public abstract bool RequiresImmediateAssignment();
    }
}
