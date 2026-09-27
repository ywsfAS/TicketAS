using Core.Exceptions;
using Core.Incidents;

namespace Core.Tests.Incidents
{
    public class IncidentDeadlineTests
    {
        [Fact]
        public void Create_WithPositiveDuration_Succeeds()
        {
            var duration = TimeSpan.FromHours(1);
            var deadline = IncidentDeadline.Create(duration);

            Assert.Equal(duration, deadline.Duration);
        }

        [Fact]
        public void Create_WithZeroDuration_Throws() =>
            Assert.Throws<IncidentDeadlineInvalidDurationException>(
                () => IncidentDeadline.Create(TimeSpan.Zero));

        [Fact]
        public void Create_WithNegativeDuration_Throws() =>
            Assert.Throws<IncidentDeadlineInvalidDurationException>(
                () => IncidentDeadline.Create(TimeSpan.FromMinutes(-1)));
    }

    public class IncidentSlaTests
    {
        private static IncidentDeadline ADeadline() =>
            IncidentDeadline.Create(TimeSpan.FromMinutes(15));

        private static IncidentDeadline BDeadline() =>
            IncidentDeadline.Create(TimeSpan.FromMinutes(30));

        [Fact]
        public void Create_WithBothDeadlines_Succeeds()
        {
            var ack = ADeadline();
            var res = ADeadline();

            var sla = IncidentSla.Create(ack, res);

            Assert.Equal(ack, sla.Acknowledgement);
            Assert.Equal(res, sla.Resolution);
        }

        [Fact]
        public void Create_WithNullAcknowledgement_Throws() => Assert.Throws<IncidentSlaDeadlineIsNullException>(() => IncidentSla.Create(null!, ADeadline()));

        [Fact]
        public void Create_WithNullResolution_Throws() => Assert.Throws<IncidentSlaDeadlineIsNullException>(() => IncidentSla.Create(ADeadline(), null!));

        [Fact]
        public void Create_WithAckGreaterThenResolution_Throws()
        {
            var ack = BDeadline();
            var res = ADeadline();
            Assert.Throws<IncidentSlaInvalidDeadlinesException>(() =>
             IncidentSla.Create(ack, res));

        }
    }
}
