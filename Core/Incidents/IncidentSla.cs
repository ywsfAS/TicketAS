using Core.Exceptions;
namespace Core.Incidents
{
    public sealed record IncidentDeadline
    {
        public TimeSpan Duration { get; }
        public DateTime DueAt { get; }

        private IncidentDeadline(TimeSpan duration, DateTime dueAt) => (Duration, DueAt) = (duration, dueAt);

        public static IncidentDeadline Create(TimeSpan duration , DateTime dueAt)
        {
            if(duration <= TimeSpan.Zero) throw new IncidentDeadlineInvalidDurationException(duration);

            return new IncidentDeadline(duration, dueAt);

        }

    }
    public sealed record IncidentSla
    {
        public IncidentDeadline Acknowledgement { get; }
        public IncidentDeadline Resolution { get; }

        private IncidentSla(IncidentDeadline ack, IncidentDeadline res) => (Acknowledgement,Resolution) = (ack,res);

        public static IncidentSla Create(IncidentDeadline ack , IncidentDeadline res)
        {
            if (ack == null || res == null) throw new IncidentSlaDeadlineIsNullException();

            return new IncidentSla(ack, res);

        }

    }
}
