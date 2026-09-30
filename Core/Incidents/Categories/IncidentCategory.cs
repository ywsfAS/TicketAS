using Core.Agents;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Incidents.Severity;
namespace Core.Incidents.Categories
{
    public abstract record IncidentCategory{

        public abstract string Name { get; }
        public AgentSeniority RequiredSeniority { get; }
        public IReadOnlyCollection<AgentSpecialization> RequiredSpecializations { get; }

        public SpecializationMatchRule SpecializationMatchRule { get;}
        public abstract IncidentSeverity GetMinimalSeverityLevel();
        public abstract TimeSpan GetAcknowledgeTime();
        public abstract TimeSpan GetResolutionTime();

        public IncidentCategory(SpecializationMatchRule rule,AgentSeniority requiredSeniority, IReadOnlyCollection<AgentSpecialization> requiredSpecializations) =>
            (SpecializationMatchRule,RequiredSeniority,RequiredSpecializations) = (rule,requiredSeniority,requiredSpecializations);

        public bool IsQualified(Agent agent)
        {
            if (!agent.MeetsSeniority(RequiredSeniority)) return false;

            return SpecializationMatchRule == SpecializationMatchRule.Any 
                ? RequiredSpecializations.Any(agent.HasSpecialization) 
                : RequiredSpecializations.All(agent.HasSpecialization);

        }
    };
}
