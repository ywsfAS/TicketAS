namespace Core.Agents.AgentStates
{
    public sealed class ActiveAgentState : AgentState
    {
        public override string Name => "Active";

        public override bool EnsureCanReceiveAssignment() => true;

        public override bool EnsureCanWorkOnIncident() => true;

        public override bool EnsureCanSendMessage() => true;

        public override bool CanMakeUnavailable() => true;

        public override bool CanActivate() => true;

        public override bool CanSuspend() => true;
    }
}
