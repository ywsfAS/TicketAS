
namespace Core.Agents.AgentStates
{
    public sealed class UnavailableAgentState : AgentState
    {
        public override string Name => "Unavailable";

        public override bool EnsureCanReceiveAssignment() => false;

        public override bool EnsureCanWorkOnIncident() => true;

        public override bool EnsureCanSendMessage() => true;

        public override bool CanMakeUnavailable() => false;

        public override bool CanActivate() => true;

        public override bool CanSuspend() => true;
    }
}
