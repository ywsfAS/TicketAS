using Core.Agents;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Incidents.Severity;

namespace Core.Incidents.Categories
{
    public sealed class NetworkIncident : IIncidentCategoryBehavior
    {
        public IncidentCategoryId CategoryId { get; private set; }
        public IncidentCategory Category { get; private set; }
        public string Name => "Network";
        public NetworkSymptomType Symptom { get; private set; }
        private NetworkIncident() { }
        private NetworkIncident(NetworkSymptomType symptom,SpecializationMatchRule rule,AgentSeniority seniority , IReadOnlyCollection<AgentSpecialization> specializations)
        {
            Category = IncidentCategory.Create(Name, rule, seniority, specializations);
            CategoryId = Category.Id;
            Symptom = symptom;
        }

        private NetworkIncident(IncidentCategory category, NetworkSymptomType symptom)
        {
            Category = category;
            CategoryId = category.Id;
            Symptom = symptom;
        }

        public static NetworkIncident Create(IncidentCategory category, NetworkSymptomType symptom)
        {
            if (category is null) throw new IncidentCategoryIsNullException();
            if (!string.Equals(category.Name, "Network", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Network symptoms can only be used with the Network category.", nameof(category));

            return new NetworkIncident(category, symptom);
        }
        public static NetworkIncident Create(NetworkSymptomType symptom,SpecializationMatchRule rule, AgentSeniority seniority , IReadOnlyCollection<AgentSpecialization> specializations)
        {
            if (seniority == null) throw new IncidentCategorySeniorityIsNullException();
            if (specializations == null) throw new IncidentCategorySpecializationIsNullException();
            
            return new NetworkIncident(symptom,rule,seniority,specializations);
        }
        public IncidentSeverity GetMinimalSeverityLevel()
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
        public TimeSpan GetAcknowledgeTime()
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
        public TimeSpan GetResolutionTime()
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
