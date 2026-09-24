using Core.Incidents.Severity;

namespace Core.Incidents.Environments
{
    public abstract class InfrastructureEnvironment 
    {
        public abstract IncidentSeverity GetMinimalSeverityLevel();
        public abstract TimeSpan GetAcknowledgeTime();
        public abstract TimeSpan GetResolutionTime();
        public abstract bool AllowsAnonymousReporter { get; }
        public abstract bool RequiresHealthySystemBeforeResolution { get; }
        public abstract bool RequiresIncidentCommander(IncidentSeverity severity);
    }
}
