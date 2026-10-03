using Core.Exceptions;
namespace Core.Incidents
{
    public sealed record IncidentDeadline
    {
        public TimeSpan Duration { get; }
        public DateTime DueAt { get; }

        private IncidentDeadline(TimeSpan duration, DateTime dueAt) => (Duration, DueAt) = (duration, dueAt);

        public static IncidentDeadline Create(TimeSpan duration)
        {
            if(duration <= TimeSpan.Zero) throw new IncidentDeadlineInvalidDurationException(duration);
            var dueAt = DateTime.UtcNow.Add(duration);

            return new IncidentDeadline(duration,dueAt);

        }

    }
    public sealed record IncidentSla
    {
        public IncidentDeadline Acknowledgement { get; }
        public IncidentDeadline Resolution { get; }

        private IncidentSla() { }
        private IncidentSla(IncidentDeadline ack, IncidentDeadline res) => (Acknowledgement,Resolution) = (ack,res);

        public static IncidentSla Create(IncidentDeadline ack , IncidentDeadline res)
        {
            if (ack == null || res == null) throw new IncidentSlaDeadlineIsNullException();
            if (ack.DueAt > res.DueAt) throw new IncidentSlaInvalidDeadlinesException("Incident SLA acknowledgement deadline cannot be later than the resolution deadline."); 

            return new IncidentSla(ack, res);

        }

    }
}
