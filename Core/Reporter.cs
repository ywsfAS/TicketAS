using Core.Incidents;
using Core.Utilities;

namespace Core
{
    public sealed record ReporterId(Guid Id) : StrongTypedId(Id);
    public class Reporter : Entity<ReporterId>
    {


        private List<Incident> _Incidents = new List<Incident>();
        public IReadOnlyCollection<Incident> Incidents => _Incidents;

    }
}
