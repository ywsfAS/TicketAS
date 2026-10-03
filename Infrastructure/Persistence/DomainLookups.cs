using Core.Agents.AgentStates;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;
using Core.Reporters.ReportStates;
using Core.Tickets.TicketLifecycle;
using Core.Tickets.TicketPriotities;
using Core.Users.UserStates;
using Core.Agents;
using Core.Tickets.Messages;
using Core.Reporters;

namespace Infrastructure.Persistence
{
    internal static class DomainLookups
    {
        private static T Find<T>(IEnumerable<T> items, Func<T, string> name, string value, string what) =>
            items.FirstOrDefault(i => name(i) == value)
            ?? throw new InvalidOperationException($"Unknown {what} '{value}' in database.");

        public static InfrastructureEnvironment Environment(string value) =>
            Find<InfrastructureEnvironment>(
                [new DevelopmentEnvironment(), new StagingEnvironment(), new ProductionEnvironment()],
                x => x.Name, value, "environment");

        public static IncidentScope Scope(string value) =>
            Find<IncidentScope>(
                [new OrganizationScope(), new DepartmentScope(), new SingleUserScope()],
                x => x.Name, value, "incident scope");

        public static IncidentSeverity Severity(string value) =>
            Find<IncidentSeverity>(
                [new LowIncidentSeverity(), new MeduimIncidentSeverity(), new HighIncidentSeverity(), new CriticalIncidentSeverity()],
                x => x.Name, value, "incident severity");

        public static UserState UserState(string value) =>
            Find<UserState>(
                [new ActiveUser(), new DeactivatedUser(), new LockedUser()],
                x => x.Name, value, "user state");

        public static ReporterState ReporterState(string value) =>
            Find<ReporterState>(
                [new ActiveReporterState(), new SuspendedReporterState(), new BlockedReporterState()],
                x => x.Name, value, "reporter state");

        public static AgentState AgentState(string value) =>
            Find<AgentState>(
                [new ActiveAgentState(), new UnavailableAgentState(), new SuspendedAgentState()],
                x => x.Name, value, "agent state");

        public static TicketLifecycle Lifecycle(string value) =>
            Find<TicketLifecycle>(
                [TicketLifecycle.Open, TicketLifecycle.InProgress, TicketLifecycle.Resolved, TicketLifecycle.Closed],
                x => x.Name, value, "ticket lifecycle");

        public static TicketPriority Priority(int level) =>
            new TicketPriority[] { new LowTicket(), new NormalTicket(), new HighTicket(), new CriticalTicket() }
                .FirstOrDefault(p => p.Level == level)
            ?? throw new InvalidOperationException($"Unknown ticket priority level {level} in database.");

        public static string ParticipantToDb(ConversationParticipantId id) =>
            (id is AgentId ? "A:" : "R:") + id.Id.ToString("D");

        public static ConversationParticipantId ParticipantFromDb(string value)
        {
            var guid = Guid.Parse(value.AsSpan(2));
            return value[0] switch
            {
                'A' => new AgentId(guid),
                'R' => new ReporterId(guid),
                _ => throw new InvalidOperationException($"Bad participant id '{value}' in database.")
            };
        }
    }
}

