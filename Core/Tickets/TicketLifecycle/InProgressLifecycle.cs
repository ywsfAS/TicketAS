using Core.Agents;
using Core.Exceptions;
using Core.Tickets.Messages;
using Core.Tickets.TicketPriotities;

namespace Core.Tickets.TicketLifecycle
{
    internal sealed record InProgressLifecycle : TicketLifecycle
    {
        public override string Name => "InProgress";

        public override void Resolve(Ticket ticket) => ticket.SetLifecycle(Resolved);
        public override void AssignAgent(Ticket ticket, Agent agent)
        {
            ticket.SetAgent(agent);
            ticket.Conversation.AddParticipant(agent);
        }
        public override void ChangePriority(Ticket ticket, TicketPriority priority) => ticket.SetPriority(priority);
        public override void ReporterSends(Ticket ticket, MessageContent content)
        {
            if (!ticket.Reporter.State.EnsureCanSendMessage()) throw new ReporterInvalidActionForStateException();
            ticket.Conversation.ReporterSends(content);
            ticket.Update();
        }
        public override void AgentSends(Ticket ticket, MessageContent content)
        {
            if (!ticket.Agent.State.EnsureCanSendMessage()) throw new AgentInvalidActionForStateException();
            ticket.Conversation.AgentSends(ticket.Agent,content);
            ticket.Update();
        }
    }
}
