using Core.Agents;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Incidents;
using Core.Incidents.Categories;
using Core.Incidents.Environments;
using Core.Incidents.Scope;
using Core.Incidents.Severity;
using Core.Reporters;
using Core.Users;

namespace Core.Tests.Incidents
{
    public class IncidentTests
    {
        private static Reporter AReporter() =>
            Reporter.Create(User.Create(
                UserName.Create("John Doe"),
                Email.Create("john@example.com"),
                PhoneNumber.Create("+1234567890")));

        private static NetworkIncident ACategory(NetworkSymptomType symptom = NetworkSymptomType.Outage) =>
            NetworkIncident.Create(symptom, SpecializationMatchRule.Any, AgentSeniority.Junior,
                new[] { AgentSpecialization.Network });

        private static Incident AnIncident(
            NetworkIncident? category = null,
            Reporter? reporter = null,
            IncidentScope? scope = null,
            InfrastructureEnvironment? environment = null) =>
            Incident.Create(
                IncidentTitle.Create("Outage", "Payments"),
                IncidentDescription.Create("Users cannot process payments right now."),
                reporter ?? AReporter(),
                category ?? ACategory(),
                scope ?? new SingleUserScope(),
                environment ?? new ProductionEnvironment());

        [Fact]
        public void Create_WithNullTitle_Throws()
        {
            Assert.Throws<IncidentTitleIsNullException>(() =>
                Incident.Create(null!, IncidentDescription.Create("A valid description here."),
                    AReporter(), ACategory(), new SingleUserScope(), new ProductionEnvironment()));
        }

        [Fact]
        public void Create_WithNullDescription_Throws()
        {
            Assert.Throws<IncidentDescriptionIsNullException>(() =>
                Incident.Create(IncidentTitle.Create("Outage", "Payments"), null!,
                    AReporter(), ACategory(), new SingleUserScope(), new ProductionEnvironment()));
        }

        [Fact]
        public void Create_WithNullReporter_Throws()
        {
            Assert.Throws<ReporterIsNullException>(() =>
                Incident.Create(IncidentTitle.Create("Outage", "Payments"),
                    IncidentDescription.Create("A valid description here."),
                    null!, ACategory(), new SingleUserScope(), new ProductionEnvironment()));
        }

        [Fact]
        public void Create_WithNullCategory_Throws()
        {
            Assert.Throws<IncidentCategoryIsNullException>(() =>
                Incident.Create(IncidentTitle.Create("Outage", "Payments"),
                    IncidentDescription.Create("A valid description here."),
                    AReporter(), null!, new SingleUserScope(), new ProductionEnvironment()));
        }

        [Fact]
        public void Create_WithNullScope_Throws()
        {
            Assert.Throws<IncidentScopeInNullException>(() =>
                Incident.Create(IncidentTitle.Create("Outage", "Payments"),
                    IncidentDescription.Create("A valid description here."),
                    AReporter(), ACategory(), null!, new ProductionEnvironment()));
        }

        [Fact]
        public void Create_WithNullEnvironment_Throws()
        {
            Assert.Throws<IncidentInfrastructureEnvironmentIsNullException>(() =>
                Incident.Create(IncidentTitle.Create("Outage", "Payments"),
                    IncidentDescription.Create("A valid description here."),
                    AReporter(), ACategory(), new SingleUserScope(), null!));
        }

        [Fact]
        public void Create_TakesWorstCaseSeverityAcrossCategoryScopeAndEnvironment()
        {
            var incident = AnIncident(
                category: ACategory(NetworkSymptomType.Outage),
                scope: new SingleUserScope(),
                environment: new ProductionEnvironment());

            Assert.IsType<CriticalIncidentSeverity>(incident.IncidentSeverity);
        }

        [Fact]
        public void Create_TakesTightestSlaDeadlinesAcrossCategoryScopeAndEnvironment()
        {
            var incident = AnIncident(
                category: ACategory(NetworkSymptomType.Outage),
                scope: new SingleUserScope(),
                environment: new ProductionEnvironment());

            Assert.Equal(TimeSpan.FromMinutes(15), incident.IncidentSla.Acknowledgement.Duration);
            Assert.Equal(TimeSpan.FromHours(2), incident.IncidentSla.Resolution.Duration);
        }

        [Fact]
        public void ChangeTitle_WithValidTitle_Succeeds()
        {
            var incident = AnIncident();
            var newTitle = IncidentTitle.Create("Degraded", "Payments");

            incident.ChangeTitle(newTitle);

            Assert.Equal(newTitle, incident.Title);
        }

        [Fact]
        public void ChangeTitle_WithNullTitle_Throws()
        {
            var incident = AnIncident();

            Assert.Throws<IncidentTitleIsNullException>(() => incident.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeDescription_WithValidDescription_Succeeds()
        {
            var incident = AnIncident();
            var newDescription = IncidentDescription.Create("Updated description of the incident.");

            incident.ChangeDescription(newDescription);

            Assert.Equal(newDescription, incident.Description);
        }

        [Fact]
        public void ChangeDescription_WithNullDescription_Throws()
        {
            var incident = AnIncident();

            Assert.Throws<IncidentDescriptionIsNullException>(() => incident.ChangeDescription(null!));
        }

        [Fact]
        public void ChangeScope_WithNullScope_Throws()
        {
            var incident = AnIncident();

            Assert.Throws<IncidentScopeInNullException>(() => incident.ChangeScope(null!));
        }

        [Fact]
        public void ChangeScope_RecalculatesSeverityAndSla()
        {
            var incident = AnIncident(
                category: ACategory(NetworkSymptomType.SlowConnection),
                scope: new SingleUserScope(),
                environment: new DevelopmentEnvironment());

            Assert.IsType<LowIncidentSeverity>(incident.IncidentSeverity);

            incident.ChangeScope(new OrganizationScope());

            // OrganizationScope alone raises the floor to Critical.
            Assert.IsType<CriticalIncidentSeverity>(incident.IncidentSeverity);
            Assert.Equal(TimeSpan.FromMinutes(15), incident.IncidentSla.Acknowledgement.Duration);
        }

        [Fact]
        public void ChangeEnvironment_WithNullEnvironment_Throws()
        {
            var incident = AnIncident();

            Assert.Throws<IncidentInfrastructureEnvironmentIsNullException>(() => incident.ChangeEnvironment(null!));
        }

        [Fact]
        public void ChangeEnvironment_RecalculatesSeverityAndSla()
        {
            var incident = AnIncident(
                category: ACategory(NetworkSymptomType.SlowConnection),
                scope: new SingleUserScope(),
                environment: new DevelopmentEnvironment());

            Assert.IsType<LowIncidentSeverity>(incident.IncidentSeverity);

            incident.ChangeEnvironment(new ProductionEnvironment());

            // ProductionEnvironment alone raises the floor to Medium.
            Assert.IsType<MeduimIncidentSeverity>(incident.IncidentSeverity);
        }
    }
}
