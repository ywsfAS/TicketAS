using Core.Exceptions;
using Core.Incidents;
using Core.Reporters.ReportStates;
using Core.Tickets.Messages;
using Core.Users;
using Core.Utilities;

namespace Core.Reporters
{
    public sealed record ReporterId(Guid Id) : ConversationParticipantId(Id);
    public class Reporter : Entity<ReporterId>
    {
        public User User { get; private set; }
        public UserId UserId { get; private set; }

        private readonly List<Incident> _Incidents = new List<Incident>();
        public ReporterState State { get; private set; }
        public IReadOnlyCollection<Incident> Incidents => _Incidents;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Reporter(User user,ReporterState state, DateTime createdAt, DateTime? updatedAt) =>
            (User,State,CreatedAt,UpdatedAt) = (user,state,createdAt,updatedAt);

        public static Reporter Create(User user)
        {
            if (user == null) throw new UserIsNullException();
            var state = new ActiveReporterState();
            return new Reporter(user,state, DateTime.UtcNow, null);
        }
        public void ReportIncident(Incident incident)
        {
            if(!State.EnsureCanReport()) throw new ReporterInvalidActionForStateException();

            _Incidents.Add(incident);
            Update();
        }
        public void Suspend()
        {
            if (!State.CanSuspend()) throw new ReporterInvalidStateTransitionException(State.Name,"Suspended");

            State = new SuspendedReporterState();
            Update();
        }

        public void Activate()
        {
            if (!State.CanActivate()) throw new ReporterInvalidStateTransitionException(State.Name,"Active");

            State = new ActiveReporterState();
            Update();
        }

        public void Block()
        {
            if (!State.CanBlock()) throw new ReporterInvalidStateTransitionException(State.Name,"Blocked");

            State = new BlockedReporterState();
            Update();
        }
        private void Update() => UpdatedAt = DateTime.UtcNow;

    }
}
