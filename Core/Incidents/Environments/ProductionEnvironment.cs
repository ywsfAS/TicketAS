using Core.Incidents.Severity;

namespace Core.Incidents.Environments
{
    public sealed class ProductionEnvironment : InfrastructureEnvironment
    {
        public override string Name => "Production";
        public override IncidentSeverity GetMinimalSeverityLevel()
            => new MeduimIncidentSeverity();

        public override TimeSpan GetAcknowledgeTime()
            => TimeSpan.FromMinutes(15);

        public override TimeSpan GetResolutionTime()
            => TimeSpan.FromHours(4);

        public override bool AllowsAnonymousReporter => false;

        public override bool RequiresHealthySystemBeforeResolution => true;

        public override bool RequiresIncidentCommander(IncidentSeverity severity)
            => severity is CriticalIncidentSeverity;
    }
}
