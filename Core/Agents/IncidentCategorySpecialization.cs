using Core.Incidents.Categories;
using Core.Utilities;

namespace Core.Agents
{
    public sealed record IncidentSpecializationId(Guid Id) : StrongTypedId(Id);
    public sealed class IncidentCategorySpecialization : Entity<IncidentSpecializationId>
    {
        public IncidentCategoryId CategoryId { get; }

        public SpecializationId SpecializationId { get; }

        public IncidentCategorySpecialization(
            IncidentCategoryId categoryId,
            SpecializationId specialization)
        {
            CategoryId = categoryId;
            SpecializationId = specialization;
        }
    }
}
