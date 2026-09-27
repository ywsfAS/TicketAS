using Core.Agents;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Reporters;
using Core.Users;

namespace Core.Tests.Reporters
{
    public class ReporterTests
    {
        private static User AUser() =>
            User.Create(
                UserName.Create("John Doe"),
                Email.Create("john@example.com"),
                PhoneNumber.Create("+1234567890"));

        private static Reporter AReporter() => Reporter.Create(AUser());

        private static Incident AnIncident(Reporter reporter) =>
            Incident.Create(
                IncidentTitle.Create("Outage", "Payments"),
                IncidentDescription.Create("Users cannot process payments right now."),
                reporter,
                NetworkIncident.Create(
                    NetworkSymptomType.Outage,
                    SpecializationMatchRule.Any,
                    AgentSeniority.Junior,
                    new[] { AgentSpecialization.Network }),
                new SingleUserScope(),
                new ProductionEnvironment());


        [Fact]
        public void Create_WithNullUser_Throws()
        {
            Assert.Throws<UserIsNullException>(() => Reporter.Create(null!));
        }

        [Fact]
        public void Create_ANewReporter_IsActiveAndHasNoIncidentsYet()
        {
            var reporter = AReporter();

            Assert.Equal("Active", reporter.State.Name);
            Assert.Empty(reporter.Incidents);
        }


        [Fact]
        public void ReportIncident_WhileActive_Succeeds()
        {
            var reporter = AReporter();
            var incident = AnIncident(reporter);

            reporter.ReportIncident(incident);

            Assert.Contains(incident, reporter.Incidents);
        }

        [Fact]
        public void ReportIncident_WhileSuspended_Throws()
        {
            var reporter = AReporter();
            reporter.Suspend();

            Assert.Throws<ReporterInvalidActionForStateException>(
                () => reporter.ReportIncident(AnIncident(reporter)));
        }

        [Fact]
        public void ReportIncident_WhileBlocked_Throws()
        {
            var reporter = AReporter();
            reporter.Block();

            Assert.Throws<ReporterInvalidActionForStateException>(
                () => reporter.ReportIncident(AnIncident(reporter)));
        }

        [Fact]
        public void ReportIncident_DoesNotAddIncidentWhenBlocked()
        {
            var reporter = AReporter();
            reporter.Block();

            try { 
                reporter.ReportIncident(AnIncident(reporter)); 
            } catch (ReporterInvalidActionForStateException) { }

            Assert.Empty(reporter.Incidents);
        }


        [Fact]
        public void Active_CanSuspend()
        {
            var reporter = AReporter();

            reporter.Suspend();

            Assert.Equal("Suspended", reporter.State.Name);
        }

        [Fact]
        public void Active_CanBlock()
        {
            var reporter = AReporter();

            reporter.Block();

            Assert.Equal("Blocked", reporter.State.Name);
        }

        [Fact]
        public void Active_CanActivate_IsANoOpTransition()
        {
            var reporter = AReporter();

            reporter.Activate();

            Assert.Equal("Active", reporter.State.Name);
        }

        [Fact]
        public void Suspended_CanActivate()
        {
            var reporter = AReporter();
            reporter.Suspend();

            reporter.Activate();

            Assert.Equal("Active", reporter.State.Name);
        }

        [Fact]
        public void Suspended_CannotSuspendAgain()
        {
            var reporter = AReporter();
            reporter.Suspend();

            Assert.Throws<ReporterInvalidStateTransitionException>(() => reporter.Suspend());
        }

        [Fact]
        public void Suspended_CannotBeBlockedDirectly()
        {
            var reporter = AReporter();
            reporter.Suspend();

            Assert.Throws<ReporterInvalidStateTransitionException>(() => reporter.Block());
        }

        [Fact]
        public void Blocked_CanActivate()
        {
            var reporter = AReporter();
            reporter.Block();

            reporter.Activate();

            Assert.Equal("Active", reporter.State.Name);
        }

        [Fact]
        public void Blocked_CannotBeSuspendedDirectly()
        {
            var reporter = AReporter();
            reporter.Block();

            Assert.Throws<ReporterInvalidStateTransitionException>(() => reporter.Suspend());
        }

        [Fact]
        public void Blocked_CannotBeBlockedAgain()
        {
            var reporter = AReporter();
            reporter.Block();

            Assert.Throws<ReporterInvalidStateTransitionException>(() => reporter.Block());
        }

        [Fact]
        public void Suspend_ThenActivate_AllowsReportingAgain()
        {
            var reporter = AReporter();
            reporter.Suspend();

            reporter.Activate();

            reporter.ReportIncident(AnIncident(reporter));
            Assert.Single(reporter.Incidents);
        }

        [Fact]
        public void Block_ThenActivate_AllowsReportingAgain()
        {
            var reporter = AReporter();
            reporter.Block();

            reporter.Activate();

            reporter.ReportIncident(AnIncident(reporter));
            Assert.Single(reporter.Incidents);
        }
    }
}