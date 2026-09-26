
namespace Core.Agents.AgentStates
{
    public class SuspendedAgentState : AgentState
    {
        public override string Name => "Suspended";

        public override bool EnsureCanReceiveAssignment() => false;

        public override bool EnsureCanWorkOnIncident() => false;

        public override bool EnsureCanSendMessage() => false;

        public override bool CanMakeUnavailable() => false;

        public override bool CanActivate() => true;

        public override bool CanSuspend() => true;
    }
}
