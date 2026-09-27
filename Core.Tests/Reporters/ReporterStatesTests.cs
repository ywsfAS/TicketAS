using Core.Reporters.ReportStates;

namespace Core.Tests.Reporters
{ 
    public class ReporterStateTests
    {
        [Fact]
        public void Active_AllowsReportingAndMessaging()
        {
            var state = new ActiveReporterState();

            Assert.Equal("Active", state.Name);
            Assert.True(state.EnsureCanReport());
            Assert.True(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Active_AllowsAllTransitions()
        {
            var state = new ActiveReporterState();

            Assert.True(state.CanSuspend());
            Assert.True(state.CanActivate());
            Assert.True(state.CanBlock());
        }

        [Fact]
        public void Suspended_DisallowsReportingAndMessaging()
        {
            var state = new SuspendedReporterState();

            Assert.Equal("Suspended", state.Name);
            Assert.False(state.EnsureCanReport());
            Assert.False(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Suspended_OnlyAllowsReactivation()
        {
            var state = new SuspendedReporterState();

            Assert.False(state.CanSuspend());
            Assert.True(state.CanActivate());
            Assert.False(state.CanBlock());
        }

        [Fact]
        public void Blocked_DisallowsReportingAndMessaging()
        {
            var state = new BlockedReporterState();

            Assert.Equal("Blocked", state.Name);
            Assert.False(state.EnsureCanReport());
            Assert.False(state.EnsureCanSendMessage());
        }

        [Fact]
        public void Blocked_OnlyAllowsReactivation()
        {
            var state = new BlockedReporterState();

            Assert.False(state.CanSuspend());
            Assert.True(state.CanActivate());
            Assert.False(state.CanBlock());
        }
    }
}
