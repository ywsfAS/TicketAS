using Core.Agents;
using Core.Exceptions;
using Core.Incidents;
using Core.Reporters;
using Core.Tickets;
using Core.Tickets.Conversation;
using Core.Tickets.Messages;
using Core.Tickets.TicketLifecycle;
using Core.Tickets.TicketPriotities;
using Core.Utilities;
using System.Runtime.CompilerServices;

namespace Core
{
    public sealed record TicketId(Guid Id) : StrongTypedId(Id);

    public sealed class Ticket : Entity<TicketId>
    {
        public TicketTitle Title { get; private set; }
        public TicketDescription Description { get; private set; }
        public Reporter Reporter { get; private set; }
        public ReporterId ReporterId { get; private set; }

        public Agent Agent { get; private set; }
        public AgentId AgentId { get; private set; }

        public Incident Incident { get; private set; }
        public IncidentId IncidentId { get; private set; }
        public TicketPriority Priority { get; private set; }
        public Conversation Conversation { get; private set; }
        public TicketLifecycle Lifecycle { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Ticket() { }

        private Ticket(TicketTitle name, TicketDescription description, Reporter reporter,Agent agent,Incident incident,TicketPriority priority,Conversation conversation,TicketLifecycle lifecycle, DateTime createdAt, DateTime? updatedAt) => (Title,Description,Reporter,Agent,Incident,Priority,Conversation,Lifecycle,CreatedAt,UpdatedAt) 
            = (name, description, reporter, agent, incident, priority, conversation, lifecycle, createdAt, updatedAt);

        public static Ticket Create(TicketTitle name, TicketDescription description, Reporter reporter,Agent agent,Incident incident,TicketPriority priority)
        {
            if (name == null) throw new TicketTitleIsNullException();
            if (description == null) throw new TicketDescriptionIsNullException();
            if (reporter == null) throw new ReporterIsNullException();
            if (agent == null) throw new AgentIsNullException();
            if (incident == null) throw new IncidentIsNullException();
            if (priority == null) throw new TicketPriorityIsNullException();
            if (!IsQualified(incident, agent)) throw new TicketAgentIsNotQualifiedForIncident();

            var conversation = Conversation.Create(reporter,agent);

            var lifecycle = new OpenLifecycle();
            
            
            var ticket = new Ticket(name, description, reporter, agent, incident, priority, conversation , lifecycle , DateTime.UtcNow,null);
            ticket.Id = new TicketId(Guid.NewGuid());
            return ticket;

        }
        private static bool IsQualified(Incident incident,Agent agent) => incident.Category.IsQualified(agent);

        internal void SetAgent(Agent agent)
        {
            if (agent == null) throw new AgentIsNullException();
            if (!IsQualified(Incident, agent)) throw new TicketAgentIsNotQualifiedForIncident();
            Agent = agent;
            Update();
        }
        internal void SetPriority(TicketPriority priority)
        {
            if (priority == null) throw new TicketPriorityIsNullException();

            Priority = priority;
            Update();

        }

        internal void SetLifecycle(TicketLifecycle lifecycle)
        {
            if(lifecycle == null) throw new TicketLifecyleIsNullException();
            Lifecycle = lifecycle;

            Update();

        }
        public void StartWork() => Lifecycle.StartWork(this);
        public void Resolve() => Lifecycle.Resolve(this);
        public void Close() => Lifecycle.Close(this);
        public void Reopen() => Lifecycle.Reopen(this);
        public void AssignAgent(Agent agent) => Lifecycle.AssignAgent(this, agent);
        public void ChangePriority(TicketPriority priority) => Lifecycle.ChangePriority(this,priority);
        public void ReporterSends(MessageContent content) => Lifecycle.ReporterSends(this,content);
        public void AgentSends(MessageContent constent) => Lifecycle.AgentSends(this,constent);

        public void ChangeTitle(TicketTitle title)
        {
            if(title == null) throw new TicketTitleIsNullException();

            Title = title;
            Update();

        }
        public void ChangeDescription(TicketDescription description)
        {
            if(description == null) throw new TicketDescriptionIsNullException();
            Description = description;

            Update();

        }




        internal void Update() => UpdatedAt = DateTime.UtcNow;

    }
}
