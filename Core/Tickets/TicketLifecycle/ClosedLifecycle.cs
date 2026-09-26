
namespace Core.Tickets.TicketLifecycle
{
    internal sealed record ClosedLifecycle : TicketLifecycle
    {
        public override string Name => "Closed";
    }

}
