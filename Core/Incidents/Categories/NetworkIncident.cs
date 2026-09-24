
using Core.Enums;
using Core.Incidents.Severity;

namespace Core.Incidents.Categories
{
    public sealed class NetworkIncident : IncidentCategory
    {
        public NetworkSymptomType Symptom { get; private set; }
        private NetworkIncident(NetworkSymptomType symptom) => Symptom = symptom;
        public static NetworkIncident Create(NetworkSymptomType symptom) => new NetworkIncident(symptom);
        public override IncidentSeverity GetMinimalSeverityLevel()
        {
            return Symptom switch
            {
                NetworkSymptomType.Outage =>
                    new CriticalIncidentSeverity(),

                NetworkSymptomType.Intermittent =>
                    new HighIncidentSeverity(),

                NetworkSymptomType.Latency =>
                    new MeduimIncidentSeverity(),

                NetworkSymptomType.PacketLoss =>
                    new MeduimIncidentSeverity(),

                NetworkSymptomType.SlowConnection =>
                    new LowIncidentSeverity(),

                _ => throw new ArgumentOutOfRangeException(nameof(Symptom), Symptom, null)
            };

        }
        public override TimeSpan GetAcknowledgeTime()
        {
            return Symptom switch
            {
                NetworkSymptomType.Outage =>
                    TimeSpan.FromMinutes(15),

                NetworkSymptomType.Intermittent =>
                    TimeSpan.FromMinutes(30),

                NetworkSymptomType.Latency =>
                    TimeSpan.FromHours(1),

                NetworkSymptomType.PacketLoss =>
                    TimeSpan.FromHours(1),

                NetworkSymptomType.SlowConnection =>
                    TimeSpan.FromHours(4),

                _ => throw new ArgumentOutOfRangeException(nameof(Symptom), Symptom, null)
            };

        }
        public override TimeSpan GetResolutionTime()
        {
            return Symptom switch
            {
                NetworkSymptomType.Outage =>
                    TimeSpan.FromHours(2),

                NetworkSymptomType.Intermittent =>
                    TimeSpan.FromHours(4),

                NetworkSymptomType.Latency =>
                    TimeSpan.FromHours(8),

                NetworkSymptomType.PacketLoss =>
                    TimeSpan.FromHours(8),

                NetworkSymptomType.SlowConnection =>
                    TimeSpan.FromHours(24),

                _ => throw new ArgumentOutOfRangeException(nameof(Symptom), Symptom, null)
            };

        }

    };
}
