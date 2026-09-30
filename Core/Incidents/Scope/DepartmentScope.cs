using Core.Incidents.Severity;

namespace Core.Incidents.Scope
{
    public sealed class DepartmentScope : IncidentScope
    {
        public override string Name => "Departement";
        public override IncidentSeverity GetMinimalSeverityLevel()
            => new HighIncidentSeverity();

        public override TimeSpan GetAcknowledgeTime()
            => TimeSpan.FromMinutes(30);

        public override TimeSpan GetResolutionTime()
            => TimeSpan.FromHours(4);
    }
}
