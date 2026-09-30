
namespace Core.Incidents.Severity
{
    public abstract class IncidentSeverity()
    {
        public abstract string Name { get; }
        public abstract int Level { get; }
    }
}
