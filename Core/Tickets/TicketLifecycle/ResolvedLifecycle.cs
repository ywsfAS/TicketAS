using Core.Tickets.Messages;

namespace Core.Tickets.TicketLifecycle
{
    internal sealed record ResolvedLifecycle : TicketLifecycle
    {
        public override string Name => "Resolved";

        public override void Close(Ticket ticket) => ticket.SetLifecycle(Closed);
        public override void Reopen(Ticket ticket) => ticket.SetLifecycle(InProgress);

        public override void ReporterSends(Ticket ticket, MessageContent content)
        {
            ticket.ReporterSends(content);
            ticket.SetLifecycle(InProgress);

            ticket.Update();
        }

        public override void AgentSends(Ticket ticket, MessageContent content)
        {
            ticket.Conversation.AgentSends(content);
            ticket.Update();
        }
    }
}
