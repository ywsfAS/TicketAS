
namespace Core.Incidents.Severity
{
    public sealed class HighIncidentSeverity : IncidentSeverity
    {
        public override string Name => "High";
        public override int Level { get; } = 3;
    }
}
