using Core.Agents;
using Core.Agents.Seniority;
using Core.Enums;
using Core.Exceptions;
using Core.Utilities;
namespace Core.Incidents.Categories
{
    public sealed record IncidentCategoryId(Guid Id) : StrongTypedId(Id);
    public class IncidentCategory : Entity<IncidentCategoryId> 
    {
        public string Name { get; private set; }
        public AgentSeniority RequiredSeniority { get; private set; }
        public IReadOnlyCollection<AgentSpecialization> RequiredSpecializations { get; }

        public SpecializationMatchRule SpecializationMatchRule { get;}

        private IncidentCategory(SpecializationMatchRule rule,AgentSeniority requiredSeniority, IReadOnlyCollection<AgentSpecialization> requiredSpecializations) =>
            (SpecializationMatchRule,RequiredSeniority,RequiredSpecializations) = (rule,requiredSeniority,requiredSpecializations);

        public static IncidentCategory Create(SpecializationMatchRule rule,AgentSeniority seniority, IReadOnlyCollection<AgentSpecialization> specializations)
        {
            if (specializations is null) throw new IncidentCategorySpecializationIsNullException();
            if (seniority == null) throw new IncidentCategorySeniorityIsNullException(); 

            return new IncidentCategory(rule,seniority,specializations);        

        }

        public bool IsQualified(Agent agent)
        {
            if (!agent.MeetsSeniority(RequiredSeniority)) return false;

            return SpecializationMatchRule == SpecializationMatchRule.Any 
                ? RequiredSpecializations.Any(agent.HasSpecialization) 
                : RequiredSpecializations.All(agent.HasSpecialization);

        }
    };
}
