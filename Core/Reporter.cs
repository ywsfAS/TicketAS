using Core.Incidents;
using Core.Tickets.Messages;
using Core.Utilities;

namespace Core
{
    public sealed record ReporterId(Guid Id) : ConversationParticipantId(Id);
    public class Reporter : Entity<ReporterId>
    {


        private List<Incident> _Incidents = new List<Incident>();
        public IReadOnlyCollection<Incident> Incidents => _Incidents;

    }
}
