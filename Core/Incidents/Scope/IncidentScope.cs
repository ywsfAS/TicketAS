using Core.Incidents.Severity;

namespace Core.Incidents.Scope
{
    public abstract class IncidentScope
    {
        public abstract string Name { get; }
        public abstract IncidentSeverity GetMinimalSeverityLevel();
        public abstract TimeSpan GetAcknowledgeTime();
        public abstract TimeSpan GetResolutionTime();
    }
}
