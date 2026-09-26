using Core.Agents;
using Core.Exceptions;
using Core.Tickets.Messages;
using Core.Tickets.TicketPriotities;

namespace Core.Tickets.TicketLifecycle
{
    public abstract record TicketLifecycle
    {
        public static readonly TicketLifecycle Open = new OpenLifecycle();
        public static readonly TicketLifecycle InProgress = new InProgressLifecycle();
        public static readonly TicketLifecycle Resolved = new ResolvedLifecycle();
        public static readonly TicketLifecycle Closed = new ClosedLifecycle();

        public abstract string Name { get; }

        public virtual void StartWork(Ticket ticket) => Reject("start work");
        public virtual void Resolve(Ticket ticket) => Reject("resolve");
        public virtual void Close(Ticket ticket) => Reject("close");
        public virtual void Reopen(Ticket ticket) => Reject("reopen");

        public virtual void AssignAgent(Ticket ticket, Agent agent) => Reject("assign an agent");
        public virtual void ChangePriority(Ticket ticket, TicketPriority priority) => Reject("change priority");
        public virtual void ReporterSends(Ticket ticket, MessageContent content) => Reject("send a reporter message");
        public virtual void AgentSends(Ticket ticket, MessageContent content) => Reject("send an agent message");

        private void Reject(string action) =>
            throw new TicketInvalidActionWithinLifecycleException(action,Name);   
    }
}
