using Core.Agents;
using Core.Tickets.Messages;
using Core.Tickets.TicketPriotities;

namespace Core.Tickets.TicketLifecycle
{
    internal sealed record OpenLifecycle : TicketLifecycle
    {
        public override string Name => "Open";

        public override void StartWork(Ticket ticket) => ticket.SetLifecycle(InProgress);
        public override void AssignAgent(Ticket ticket, Agent agent) => ticket.SetAgent(agent);
        public override void ChangePriority(Ticket ticket, TicketPriority priority) => ticket.SetPriority(priority);
        public override void ReporterSends(Ticket ticket, MessageContent content)
        {
            ticket.ReporterSends(content);
            ticket.Update();
        }
        public override void AgentSends(Ticket ticket, MessageContent content)
        {
            ticket.Conversation.AgentSends(content);
            ticket.Update();
        }
    }
}
