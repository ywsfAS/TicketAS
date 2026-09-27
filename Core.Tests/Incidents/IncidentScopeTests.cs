using Core.Incidents.Scope;
using Core.Incidents.Severity;

namespace Core.Tests.Incidents
{
    public class IncidentScopeTests
    {
        [Fact]
        public void SingleUserScope_HasLowSeverityAndLenientDeadlines()
        {
            var scope = new SingleUserScope();

            Assert.IsType<LowIncidentSeverity>(scope.GetMinimalSeverityLevel());
            Assert.Equal(TimeSpan.FromHours(4), scope.GetAcknowledgeTime());
            Assert.Equal(TimeSpan.FromHours(24), scope.GetResolutionTime());
        }

        [Fact]
        public void DepartmentScope_HasHighSeverityAndModerateDeadlines()
        {
            var scope = new DepartmentScope();

            Assert.IsType<HighIncidentSeverity>(scope.GetMinimalSeverityLevel());
            Assert.Equal(TimeSpan.FromMinutes(30), scope.GetAcknowledgeTime());
            Assert.Equal(TimeSpan.FromHours(4), scope.GetResolutionTime());
        }

        [Fact]
        public void OrganizationScope_HasCriticalSeverityAndTightestDeadlines()
        {
            var scope = new OrganizationScope();

            Assert.IsType<CriticalIncidentSeverity>(scope.GetMinimalSeverityLevel());
            Assert.Equal(TimeSpan.FromMinutes(15), scope.GetAcknowledgeTime());
            Assert.Equal(TimeSpan.FromHours(2), scope.GetResolutionTime());
        }

        [Fact]
        public void SeverityRises_AsScopeWidens()
        {
            var singleUser = new SingleUserScope().GetMinimalSeverityLevel().Level;
            var department = new DepartmentScope().GetMinimalSeverityLevel().Level;
            var organization = new OrganizationScope().GetMinimalSeverityLevel().Level;

            Assert.True(singleUser < department);
            Assert.True(department < organization);
        }
    }
}
