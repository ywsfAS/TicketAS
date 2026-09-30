using Core.Incidents.Categories;

namespace Core.Agents
{
    public sealed class IncidentCategorySpecialization
    {
        public IncidentCategoryId CategoryId { get; }

        public AgentSpecialization Specialization { get; }

        public IncidentCategorySpecialization(
            IncidentCategoryId categoryId,
            AgentSpecialization specialization)
        {
            CategoryId = categoryId;
            Specialization = specialization;
        }
    }
}
