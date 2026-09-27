using Core.Incidents.Environments;
using Core.Incidents.Severity;

namespace Core.Tests.Incidents
{
    public class InfrastructureEnvironmentTests
    {
        [Fact]
        public void DevelopmentEnvironment_AllowsAnonymousReporterAndHasLenientRules()
        {
            var env = new DevelopmentEnvironment();

            Assert.IsType<LowIncidentSeverity>(env.GetMinimalSeverityLevel());
            Assert.Equal(TimeSpan.FromDays(1), env.GetAcknowledgeTime());
            Assert.Equal(TimeSpan.FromDays(1), env.GetResolutionTime());
            Assert.True(env.AllowsAnonymousReporter);
            Assert.False(env.RequiresHealthySystemBeforeResolution);
            Assert.False(env.RequiresIncidentCommander(new CriticalIncidentSeverity()));
        }

        [Fact]
        public void StagingEnvironment_AllowsAnonymousReporterAndHasLenientRules()
        {
            var env = new StagingEnvironment();

            Assert.IsType<LowIncidentSeverity>(env.GetMinimalSeverityLevel());
            Assert.True(env.AllowsAnonymousReporter);
            Assert.False(env.RequiresHealthySystemBeforeResolution);
            Assert.False(env.RequiresIncidentCommander(new CriticalIncidentSeverity()));
        }

        [Fact]
        public void ProductionEnvironment_DoesNotAllowAnonymousReporter()
        {
            var env = new ProductionEnvironment();

            Assert.IsType<MeduimIncidentSeverity>(env.GetMinimalSeverityLevel());
            Assert.Equal(TimeSpan.FromMinutes(15), env.GetAcknowledgeTime());
            Assert.Equal(TimeSpan.FromHours(4), env.GetResolutionTime());
            Assert.False(env.AllowsAnonymousReporter);
            Assert.True(env.RequiresHealthySystemBeforeResolution);
        }

        [Fact]
        public void ProductionEnvironment_RequiresIncidentCommander_OnlyForCriticalSeverity()
        {
            var env = new ProductionEnvironment();

            Assert.True(env.RequiresIncidentCommander(new CriticalIncidentSeverity()));
            Assert.False(env.RequiresIncidentCommander(new HighIncidentSeverity()));
            Assert.False(env.RequiresIncidentCommander(new MeduimIncidentSeverity()));
            Assert.False(env.RequiresIncidentCommander(new LowIncidentSeverity()));
        }
    }
}
