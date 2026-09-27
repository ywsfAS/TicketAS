using Core.Exceptions;
using Core.Incidents;

namespace Core.Tests.Incidents
{
    public class IncidentTitleTests
    {
        [Fact]
        public void Create_WithProblemAndService_BuildsExpectedTitle()
        {
            var title = IncidentTitle.Create("Outage", "Payments");

            Assert.Equal("Outage", title.Problem);
            Assert.Equal("Payments", title.AffectedService);
            Assert.Equal("[Outage] - [Payments]", title.Title);
        }

        [Fact]
        public void Create_TrimsProblemAndService()
        {
            var title = IncidentTitle.Create("  Outage  ", "  Payments  ");

            Assert.Equal("Outage", title.Problem);
            Assert.Equal("Payments", title.AffectedService);
        }

        [Fact]
        public void Create_WithNullProblem_Throws()
        {
            Assert.Throws<IncidentTitleProblemIsNullException>(() => IncidentTitle.Create(null!, "Payments"));
        }

        [Fact]
        public void Create_WithNullService_Throws()
        {
            Assert.Throws<IncidentTitleServiceIsNullException>(() => IncidentTitle.Create("Outage", null!));
        }

        [Fact]
        public void Create_WithProblemContainingTheWordService_Throws()
        {
            Assert.Throws<IncidentTitleProblemIsInvalidException>(
                () => IncidentTitle.Create("service", "Payments"));
        }

        [Fact]
        public void Create_WithServiceContainingTheWordProblem_Throws()
        {
            Assert.Throws<IncidentTitleServiceIsInvalidException>(
                () => IncidentTitle.Create("Outage", "problem"));
        }

        [Fact]
        public void Create_WithDashInProblem_Throws()
        {
            Assert.Throws<IncidentTitleProblemIsInvalidException>(
                () => IncidentTitle.Create("Out-age", "Payments"));
        }

        [Fact]
        public void Create_WithSlashInService_Throws()
        {
            Assert.Throws<IncidentTitleServiceIsInvalidException>(
                () => IncidentTitle.Create("Outage", "Pay/ments"));
        }

        [Fact]
        public void Create_WithEmptyProblem_Throws()
        {
            Assert.Throws<IncidentTitleProblemIsInvalidException>(() => IncidentTitle.Create("", "Payments"));
        }

        [Fact]
        public void Create_WithProblemLongerThanMax_Throws()
        {
            var tooLong = new string('a', 201);

            Assert.Throws<IncidentTitleProblemIsInvalidException>(() => IncidentTitle.Create(tooLong, "Payments"));
        }

        [Fact]
        public void Create_WithServiceLongerThanMax_Throws()
        {
            var tooLong = new string('a', 101);

            Assert.Throws<IncidentTitleServiceIsInvalidException>(() => IncidentTitle.Create("Outage", tooLong));
        }

        [Fact]
        public void Create_FromSingleString_SplitsOnFirstDash()
        {
            var title = IncidentTitle.Create("Login failure-Auth Service");

            Assert.Equal("Login failure", title.Problem);
            Assert.Equal("Auth Service", title.AffectedService);
        }

        [Fact]
        public void Create_FromSingleStringWithNoDash_Throws()
        {
            Assert.Throws<IncidentTitleException>(() => IncidentTitle.Create("No dash here at all"));
        }

        [Fact]
        public void Create_FromNullSingleString_Throws()
        {
            Assert.Throws<IncidentTitleIsNullException>(() => IncidentTitle.Create((string)null!));
        }
    }
}
