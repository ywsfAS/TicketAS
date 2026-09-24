
namespace Core.Incidents.Severity
{
    public sealed class LowIncidentSeverity: IncidentSeverity
    {
        public override int Level { get; } = 1;
    }
}
