
namespace Core.Incidents.Severity
{
    public static class IncidentSeverityExtension {
        public static IncidentSeverity Max(this IEnumerable<IncidentSeverity> list)
        {
            var max = list.Max(s => s.Level);

            return max switch
            {
                1 => new LowIncidentSeverity(),
                2 => new MeduimIncidentSeverity(),
                3 => new HighIncidentSeverity(),
                4 => new CriticalIncidentSeverity(),
                _ => throw new ArgumentOutOfRangeException(nameof(list))
            };
        }
    }
}
