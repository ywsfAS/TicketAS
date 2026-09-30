using Core.Incidents.Severity;

namespace Core.Incidents.Categories
{
    public interface IIncidentCategoryBehavior
    {
        IncidentSeverity GetMinimalSeverityLevel();

        TimeSpan GetAcknowledgeTime();

        TimeSpan GetResolutionTime();
    }
}
