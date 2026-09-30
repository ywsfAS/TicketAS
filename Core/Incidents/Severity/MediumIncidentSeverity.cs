namespace Core.Incidents.Severity
{
    public sealed class MeduimIncidentSeverity : IncidentSeverity
    {
        public override string Name => "Meduim";
        public override int Level { get; } = 2;

    }
}
