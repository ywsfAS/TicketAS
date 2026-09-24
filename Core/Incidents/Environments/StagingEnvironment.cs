
using Core.Incidents.Severity;

namespace Core.Incidents.Environments
{
    public sealed class StagingEnvironment : InfrastructureEnvironment
    {
        public override IncidentSeverity GetMinimalSeverityLevel()
            => new LowIncidentSeverity();

        public override TimeSpan GetAcknowledgeTime()
            => TimeSpan.FromDays(1);

        public override TimeSpan GetResolutionTime()
            => TimeSpan.FromDays(1);

        public override bool AllowsAnonymousReporter => true;

        public override bool RequiresHealthySystemBeforeResolution => false;

        public override bool RequiresIncidentCommander(IncidentSeverity severity)
            => false;
    }
}
