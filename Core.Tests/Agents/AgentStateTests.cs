using Core.Agents.AgentStates;

namespace Core.Tests.Agents
{
    public class AgentStateTests
    {
        [Fact]
        public void Active_AllowsAssignmentWorkAndMessaging()
        {
            var state = new ActiveAgentState();

            Assert.Equal("Active", state.Name);
            Assert.True(state.EnsureCanReceiveAssignment());
            Assert.True(state.EnsureCanWorkOnIncident());
            Assert.True(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Active_AllowsAllTransitions()
        {
            var state = new ActiveAgentState();

            Assert.True(state.CanMakeUnavailable());
            Assert.True(state.CanActivate());
            Assert.True(state.CanSuspend());
        }

        [Fact]
        public void Suspended_DisallowsAssignmentWorkAndMessaging()
        {
            var state = new SuspendedAgentState();

            Assert.Equal("Suspended", state.Name);
            Assert.False(state.EnsureCanReceiveAssignment());
            Assert.False(state.EnsureCanWorkOnIncident());
            Assert.False(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Suspended_CannotBecomeUnavailableDirectly()
        {
            var state = new SuspendedAgentState();

            Assert.False(state.CanMakeUnavailable());
            Assert.True(state.CanActivate());
        }

        [Fact]
        public void Suspended_CanSuspendAgain()
        {
            var state = new SuspendedAgentState();

            Assert.True(state.CanSuspend());
        }

        [Fact]
        public void Unavailable_DisallowsAssignmentButAllowsWorkAndMessaging()
        {
            var state = new UnavailableAgentState();

            Assert.Equal("Unavailable", state.Name);
            Assert.False(state.EnsureCanReceiveAssignment());
            Assert.True(state.EnsureCanWorkOnIncident());
            Assert.True(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Unavailable_CannotBecomeUnavailableAgain()
        {
            var state = new UnavailableAgentState();

            Assert.False(state.CanMakeUnavailable());
        }

        [Fact]
        public void Unavailable_CanTransitionDirectlyToActiveOrSuspended()
        {
            var state = new UnavailableAgentState();

            Assert.True(state.CanActivate());
            Assert.True(state.CanSuspend());
        }
    }
}