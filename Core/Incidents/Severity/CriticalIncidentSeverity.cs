
namespace Core.Incidents.Severity
{
    public sealed class CriticalIncidentSeverity : IncidentSeverity
    {
        public override string Name => "Critical";
        public override int Level { get; } = 4;
    }
}
