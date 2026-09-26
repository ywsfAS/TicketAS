namespace Core.Agents.AgentStates
{
    public abstract class AgentState
    {
        public abstract string Name { get; }

        public abstract bool EnsureCanReceiveAssignment();

        public abstract bool EnsureCanWorkOnIncident();

        public abstract bool EnsureCanSendMessage();

        public abstract bool CanMakeUnavailable();

        public abstract bool CanActivate();

        public abstract bool CanSuspend();
    }
}
