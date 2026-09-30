using Core.Incidents.Severity;

namespace Core.Incidents.Scope
{
    public sealed class OrganizationScope : IncidentScope
    {
        public override string Name => "Organization";
        public override IncidentSeverity GetMinimalSeverityLevel()
            => new CriticalIncidentSeverity();

        public override TimeSpan GetAcknowledgeTime()
            => TimeSpan.FromMinutes(15);

        public override TimeSpan GetResolutionTime()
            => TimeSpan.FromHours(2);
    }
}
