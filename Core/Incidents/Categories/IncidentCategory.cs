using Core.Incidents.Severity;
namespace Core.Incidents.Categories
{
    public abstract class IncidentCategory{
        public abstract IncidentSeverity GetMinimalSeverityLevel();
        public abstract TimeSpan GetAcknowledgeTime();
        public abstract TimeSpan GetResolutionTime();
    };
}
