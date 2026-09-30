using Core.Incidents.Severity;

namespace Core.Incidents.Scope
{
    public sealed class SingleUserScope : IncidentScope
    {
        public override string Name => "SingleUser";
        public override IncidentSeverity GetMinimalSeverityLevel()
            => new LowIncidentSeverity();

        public override TimeSpan GetAcknowledgeTime()
            => TimeSpan.FromHours(4);

        public override TimeSpan GetResolutionTime()
            => TimeSpan.FromHours(24);
    }
}
