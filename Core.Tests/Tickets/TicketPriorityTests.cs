using Core.Tickets.TicketPriotities;

namespace Core.Tests.Tickets
{
    public class TicketPriorityTests
    {
        [Fact]
        public void Critical_RequiresImmediateAssignmentAndDisallowsQueueing()
        {
            var priority = new CriticalTicket();

            Assert.Equal(3, priority.Level);
            Assert.True(priority.RequiresImmediateAssignment());
            Assert.True(priority.RequiresAssignment());
            Assert.False(priority.AllowsUnassigned());
            Assert.False(priority.AllowsWaitInQueue());
        }

        [Fact]
        public void High_RequiresAssignmentButNotImmediately()
        {
            var priority = new HighTicket();

            Assert.Equal(2, priority.Level);
            Assert.False(priority.RequiresImmediateAssignment());
            Assert.True(priority.RequiresAssignment());
            Assert.False(priority.AllowsUnassigned());
            Assert.False(priority.AllowsWaitInQueue());
        }

        [Fact]
        public void Normal_DoesNotRequireAssignment()
        {
            var priority = new NormalTicket();

            Assert.Equal(1, priority.Level);
            Assert.False(priority.RequiresImmediateAssignment());
            Assert.False(priority.RequiresAssignment());
            Assert.True(priority.AllowsUnassigned());
            Assert.False(priority.AllowsWaitInQueue());
        }

        [Fact]
        public void Low_AllowsUnassignedAndQueueing()
        {
            var priority = new LowTicket();

            Assert.Equal(0, priority.Level);
            Assert.False(priority.RequiresImmediateAssignment());
            Assert.True(priority.RequiresAssignment());
            Assert.True(priority.AllowsUnassigned());
            Assert.True(priority.AllowsWaitInQueue());
        }

        [Fact]
        public void Levels_AreOrderedCriticalHighestToLowLowest()
        {
            Assert.True(new CriticalTicket().Level > new HighTicket().Level);
            Assert.True(new HighTicket().Level > new NormalTicket().Level);
            Assert.True(new NormalTicket().Level > new LowTicket().Level);
        }
    }
}