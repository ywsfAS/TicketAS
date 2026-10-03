using Core.Agents.AgentStates;
using Core.Incidents;
using Core.Tickets.Messages;
using Core.Users;
using Core.Utilities;
using Core.Exceptions;
using Core.Agents.Seniority;

namespace Core.Agents
{
    public sealed record AgentId(Guid Id) : ConversationParticipantId(Id);
    public class Agent : Entity<AgentId>
    {
        public User User { get; private set; }
        public UserId UserId { get; private set; }
        public AgentSeniority Seniority { get; private set; }

        private HashSet<AgentSpecialization> _agentSpecializations = new();
        public IReadOnlyCollection<AgentSpecialization> Specializations => _agentSpecializations;

        public AgentState State { get; private set; }
        private readonly List<Incident> _incidents = new List<Incident>();
        public IReadOnlyCollection<Incident> Incidents => _incidents.AsReadOnly();

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Agent(User user, AgentState state,AgentSeniority seniority, DateTime createdAt, DateTime? updatedAt) =>
            (User,State,Seniority,CreatedAt,UpdatedAt) = (user, state, seniority, createdAt, updatedAt);

        public static Agent Create(User user, AgentState state , AgentSeniority seniority)
        {
            if(user is null) throw new UserIsNullException();
            if(state is null) throw new AgentStateIsNullException();
            if (seniority is null) throw new AgentSeniorityIsNullException();

            return new Agent(user, state, seniority,DateTime.UtcNow, null);

        }
        public bool HasSpecialization(AgentSpecialization specialization) => _agentSpecializations.Contains(specialization);
        public void AddSpecialization(AgentSpecialization specialization) => _agentSpecializations.Add(specialization);
        public void RemoveSpecialization(AgentSpecialization specialization) => _agentSpecializations.Remove(specialization);

        public bool MeetsSeniority(AgentSeniority seniority) => Seniority.Meets(seniority);

        public void AssignIncident(Incident incident)
        {
            if (!State.EnsureCanReceiveAssignment()) throw new AgentInvalidActionForStateException();

            _incidents.Add(incident);
            Update();
        }

        public void MakeUnavailable()
        {
            if (!State.CanMakeUnavailable())
            {
                throw new AgentInvalidStateTransitionException(
                    State.Name,
                    "Unavailable");
            }

            State = new UnavailableAgentState();
            Update();
        }

        public void Activate()
        {
            if (!State.CanActivate())
            {
                throw new AgentInvalidStateTransitionException(
                    State.Name,
                    "Active");
            }

            State = new ActiveAgentState();
            Update();
        }

        public void Suspend()
        {
            if (!State.CanSuspend())
            {
                throw new AgentInvalidStateTransitionException(
                    State.Name,
                    "Suspended");
            }

            State = new SuspendedAgentState();
            Update();
        }


        private void Update() => UpdatedAt = DateTime.UtcNow;
    }
}
